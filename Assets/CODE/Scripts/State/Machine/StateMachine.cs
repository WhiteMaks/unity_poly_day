using System.Collections.Generic;

namespace CODE.Scripts.State.Machine
{
	/// <summary>
	/// Простейшая реализация машины состояний, управляющая корневым состоянием и последовательником переходов.
	/// </summary>
	/// <remarks>
	/// StateMachine хранит ссылку на корневое состояние (<see cref="BaseState"/>) и делегирует
	/// обновления и переходы через <see cref="TransitionSequencer"/>. Управление жизненным циклом
	/// (Start/Update) обеспечивает запуск дерева состояний и периодическое обновление.
	/// </remarks>
	public class StateMachine
	{
		private readonly BaseState _rootState;
		private readonly TransitionSequencer _transitionSequencer;

		private bool _started;

		/// <summary>
		/// Создаёт машину состояний для указанного корневого состояния.
		/// </summary>
		/// <param name="rootState">Корневое состояние дерева состояний.</param>
		public StateMachine(BaseState rootState)
		{
			_rootState = rootState;
			_transitionSequencer = new TransitionSequencer(this);
		}

		/// <summary>
		/// Запускает машину состояний: устанавливает флаг начала и вызывает Enter у корневого состояния.
		/// Вызов идемпотентен — повторный Start не повлияет на уже запущенную машину.
		/// </summary>
		private void Start()
		{
			if (_started)
			{
				return;
			}

			_started = true;

			_rootState.Enter();
		}

		/// <summary>
		/// Ежекадровый вызов обновления для машины. Если машина ещё не запущена, сначала запускает её.
		/// Делегирует актуальную работу <see cref="TransitionSequencer.Update"/>.
		/// </summary>
		public void Update()
		{
			if (!_started)
			{
				Start();
			}

			_transitionSequencer.Update();
		}

		/// <summary>
		/// Немедленно сменить состояние: выполнит выход (Exit) от <paramref name="from"/> до общего предка,
		/// затем зайдёт (Enter) в путь от общего предка до <paramref name="to"/>.
		/// </summary>
		/// <param name="from">Текущее состояние, откуда совершается переход.</param>
		/// <param name="to">Целевое состояние, в которое необходимо перейти.</param>
		public void ChangeState(BaseState from, BaseState to)
		{
			if (from == to || from == null || to == null)
			{
				return;
			}

			var commonParentState = TransitionSequencer.FindCommonParent(from, to);
			for (var state = from; state != commonParentState; state = state.GetParent())
			{
				state.Exit();
			}

			var stateStack = new Stack<BaseState>();
			for (var state = to; state != commonParentState; state = state.GetParent())
			{
				stateStack.Push(state);
			}

			while (stateStack.Count > 0)
			{
				stateStack.Pop().Enter();
			}
		}

		/// <summary>
		/// Возвращает внутренний <see cref="TransitionSequencer"/>, используемый для планирования переходов.
		/// </summary>
		public TransitionSequencer GetTransitionSequencer()
		{
			return _transitionSequencer;
		}

		/// <summary>
		/// Возвращает корневое состояние, с которым ассоциирована машина.
		/// </summary>
		public BaseState GetRootState()
		{
			return _rootState;
		}

		/// <summary>
		/// Внутренний вызов обновления — применим для последовательника переходов, когда нет активной последовательности.
		/// </summary>
		internal void InternalUpdate()
		{
			_rootState.Update();
		}
	}
}