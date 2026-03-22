using System;
using System.Collections.Generic;
using System.Threading;
using CODE.Scripts.State.Machine.Enums;
using CODE.Scripts.State.Machine.Interfaces;
using CODE.Scripts.State.Machine.Sequences;

namespace CODE.Scripts.State.Machine
{
	/// <summary>
	/// Управляет последовательностями переходов между состояниями в <see cref="StateMachine"/>.
	/// </summary>
	/// <remarks>
	/// TransitionSequencer отвечает за выполнение двух фаз при переходе:
	/// 1) Деактивация активностей состояний, которые выходят из текущего пути (Exit phase).
	/// 2) Активация активностей состояний, которые входят в новый путь (Enter phase).
	/// Переходы могут выполняться последовательно или параллельно (используется <see cref="ISequence"/>).
	/// Если во время выполнения последовательности поступает новый запрос перехода, он буферизуется
	/// и будет выполнен после завершения текущей последовательности.
	/// </remarks>
	public class TransitionSequencer
	{
		private readonly StateMachine _stateMachine;

		private const bool UseSequential = true;

		private ISequence _currentSequence;
		private Action _nextPhase;

		private BaseState _pendingFromState;
		private BaseState _pendingToState;

		private BaseState _lastFromState;
		private BaseState _lastToState;

		private CancellationTokenSource _tokenSource;

		/// <summary>
		/// Создаёт экземпляр <see cref="TransitionSequencer"/>, привязанный к переданному <see cref="StateMachine"/>.
		/// </summary>
		/// <param name="stateMachine">Инстанс машины состояний, для которой этот последовательник будет управлять переходами.</param>
		public TransitionSequencer(StateMachine stateMachine)
		{
			_stateMachine = stateMachine;
		}

		/// <summary>
		/// Запросить переход от одного состояния к другому. Если переход недействителен (равные или null), игнорируется.
		/// </summary>
		/// <param name="fromState">Исходное состояние, из которого начинается переход.</param>
		/// <param name="toState">Целевое состояние, в которое нужно перейти.</param>
		public void RequestTransition(BaseState fromState, BaseState toState)
		{
			if (fromState == toState ||  fromState == null || toState == null)
			{
				return;
			}

			if (_currentSequence != null)
			{
				_pendingFromState = fromState;
				_pendingToState = toState;
				return;
			}

			Begin(fromState, toState);
		}

		/// <summary>
		/// Обновляет выполнение текущей последовательности перехода и, если последовательность отсутствует,
		/// вызывает внутреннее обновление машины состояний.
		/// </summary>
		public void Update()
		{
			if (_currentSequence != null)
			{
				if (_currentSequence.Update())
				{
					if (_nextPhase != null)
					{
						var nextPhase = _nextPhase;
						_nextPhase = null;
						nextPhase();
					}
					else
					{
						End();
					}
				}
				return;
			}

			_stateMachine.InternalUpdate();
		}

		/// <summary>
		/// Находит ближайшего общего предка двух состояний в дереве состояний.
		/// </summary>
		/// <param name="state1">Первое состояние для поиска общего предка.</param>
		/// <param name="state2">Второе состояние для поиска общего предка.</param>
		/// <returns>Общий предок, либо <c>null</c>, если общего предка нет.</returns>
		public static BaseState FindCommonParent(BaseState state1, BaseState state2)
		{
			var tree = new HashSet<BaseState>();

			for (var state = state1; state != null; state = state.GetParent())
			{
				tree.Add(state);
			}

			for (var state = state2; state != null; state = state.GetParent())
			{
				if (tree.Contains(state))
				{
					return state;
				}
			}

			return null;
		}

		/// <summary>
		/// Формирует список шагов (делегатов <see cref="PhaseStep"/>) для деактивации активностей
		/// входящих состояний (exit-phase).
		/// </summary>
		private static List<PhaseStep> GetDeactivatePhaseSteps(List<BaseState> states)
		{
			var result = new List<PhaseStep>();

			foreach (var state in states)
			{
				var activities = state.GetActivities();
				foreach (var activity in activities)
				{
					if (activity.Mode == ActiveMode.Active)
					{
						result.Add(token => activity.DeactivateAsync(token));
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Формирует список шагов для активации активностей входящих состояний (enter-phase).
		/// </summary>
		private static List<PhaseStep> GetActivatePhaseSteps(List<BaseState> states)
		{
			var result = new List<PhaseStep>();

			foreach (var state in states)
			{
				var activities = state.GetActivities();
				foreach (var activity in activities)
				{
					if (activity.Mode == ActiveMode.Inactive)
					{
						result.Add(token => activity.ActivateAsync(token));
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Возвращает список состояний, которые должны быть покинуты при переходе от <paramref name="fromState"/>
		/// до общего предка с целевым состоянием. Порядок — от ближайшего к удалённому (вверх по дереву).
		/// </summary>
		private static List<BaseState> GetStatesToExit(BaseState fromState, BaseState toState)
		{
			var result = new List<BaseState>();

			for (var state = fromState; state != null && state != toState; state = state.GetParent()) {
				result.Add(state);
			}

			return result;
		}

		/// <summary>
		/// Возвращает список состояний, которые должны быть введены (entered) при переходе.
		/// Порядок — от родителя к дочерним (в порядке входа).
		/// </summary>
		private static List<BaseState> GetStatesToEnter(BaseState fromState, BaseState toState)
		{
			var result = new Stack<BaseState>();

			for (var state = fromState; state != null && state != toState; state = state.GetParent()) {
				result.Push(state);
			}

			return new List<BaseState>(result);
		}

		/// <summary>
		/// Начинает выполнение перехода: создаёт токен отмены и запускает фазу деактивации.
		/// По завершении деактивации будет выполнена смена состояния в машине и запущена фаза активации.
		/// </summary>
		private void Begin(BaseState fromState, BaseState toState)
		{
			_tokenSource?.Cancel();
			_tokenSource = new CancellationTokenSource();

			var commonParent = FindCommonParent(fromState, toState);
			var stateToExit = GetStatesToExit(fromState, commonParent);
			var stateToEnter = GetStatesToEnter(toState, commonParent);

			var deactivatePhaseSteps = GetDeactivatePhaseSteps(stateToExit);
			_currentSequence = GetPhaseSequence(deactivatePhaseSteps, _tokenSource.Token);
			_currentSequence.Start();

			_nextPhase = () =>
			{
				_stateMachine.ChangeState(fromState, toState);

				var activatePhaseSteps = GetActivatePhaseSteps(stateToEnter);
				_currentSequence = GetPhaseSequence(activatePhaseSteps, _tokenSource.Token);
				_currentSequence.Start();
			};
		}

		/// <summary>
		/// Завершает текущую последовательность и, если есть отложенный переход, инициирует его.
		/// </summary>
		private void End()
		{
			_currentSequence = null;

			if (_pendingFromState != null && _pendingToState != null)
			{
				var fromState = _pendingFromState;
				var toState = _pendingToState;

				_pendingFromState = null;
				_pendingToState = null;

				RequestTransition(fromState, toState);
			}
		}

		/// <summary>
		/// Возвращает экземпляр <see cref="ISequence"/>, который будет выполнять список шагов.
		/// Выбирается между последовательной и параллельной реализацией.
		/// </summary>
		private ISequence GetPhaseSequence(List<PhaseStep> steps, CancellationToken token)
		{
			return UseSequential ? new SequentialPhaseSequence(steps, token) : new ParallelPhaseSequence(steps, token);
		}
	}
}