using System.Collections.Generic;
using CODE.Scripts.State.Machine.Interfaces;

namespace CODE.Scripts.State.Machine
{
	/// <summary>
	/// Базовый класс для всех состояний в иерархической (деревянной) машине состояний.
	/// </summary>
	/// <remarks>
	/// BaseState поддерживает вложенные дочерние состояния, набор активностей (<see cref="IActivity"/>)
	/// и предоставляет точки расширения (OnEnter/OnExit/OnUpdate), которые должны переопределяться наследниками.
	/// Методы Enter/Exit/Update управляют жизненным циклом и рекурсивно применяются к дочерним состояниям.
	/// </remarks>
	public abstract class BaseState
	{
		private readonly List<IActivity> _activities = new();

		/// <summary>
		/// Ссылка на машину состояний. Может быть установлена при помощи <see cref="StateMachineBuilder"/>.
		/// </summary>
		protected readonly StateMachine StateMachine;

		/// <summary>
		/// Родительское состояние (null для корня).
		/// </summary>
		protected readonly BaseState Parent;

		/// <summary>
		/// Текущий активный дочерний стейт (если есть).
		/// </summary>
		protected BaseState CurrentChild;

		protected IReadOnlyList<IActivity> ROActivities => _activities;

		/// <summary>
		/// Защищённый конструктор для состояния. Обычно StateMachineBuilder установит StateMachine после создания.
		/// </summary>
		/// <param name="stateMachine">Экземпляр машины, может быть null до момента проводки.</param>
		/// <param name="parent">Родительское состояние в дереве.</param>
		protected BaseState(StateMachine stateMachine, BaseState parent)
		{
			StateMachine = stateMachine;
			Parent = parent;
		}

		/// <summary>
		/// Добавляет активность в состояние. Игнорирует null.
		/// </summary>
		/// <param name="activity">Активность для добавления.</param>
		public void Add(IActivity activity)
		{
			if (activity != null)
			{
				_activities.Add(activity);
			}
		}

		/// <summary>
		/// Точка расширения: вызывается при входе в состояние (Enter).
		/// Переопределять для выполнения специфичной логики при входе.
		/// </summary>
		protected virtual void OnEnter()
		{
		}

		/// <summary>
		/// Точка расширения: вызывается при выходе из состояния (Exit).
		/// Переопределять для выполнения очистки/отключения.
		/// </summary>
		protected virtual void OnExit()
		{
		}

		/// <summary>
		/// Точка расширения: регулярное обновление состояния. Вызывается после обновления дочерних состояний.
		/// </summary>
		protected virtual void OnUpdate()
		{
		}

		/// <summary>
		/// Точка расширения: LateUpdate, если требуется отдельная логика на позднем обновлении кадра.
		/// </summary>
		protected virtual void OnLateUpdate()
		{
		}

		/// <summary>
		/// Точка расширения: FixedUpdate, если требуется логика физического обновления.
		/// </summary>
		protected virtual void OnFixedUpdate()
		{
		}

		/// <summary>
		/// Возвращает самый глубокий вложенный дочерний стейт (следуя CurrentChild), или сам объект, если дочерних нет.
		/// </summary>
		public BaseState GetDeepestChild()
		{
			var result = this;

			while (result.CurrentChild != null)
			{
				result = result.CurrentChild;
			}

			return result;
		}

		/// <summary>
		/// Возвращает перечисление состояний от текущего до корня (включая текущий), полезно для трассировки пути.
		/// </summary>
		public IEnumerable<BaseState> GetPathToRoot()
		{
			var result = this;

			while (result != null)
			{
				yield return result;

				result = result.Parent;
			}
		}

		/// <summary>
		/// Возвращает родительское состояние (или null для корня).
		/// </summary>
		public BaseState GetParent()
		{
			return Parent;
		}

		/// <summary>
		/// Возвращает список активностей зарегистрированных в состоянии.
		/// </summary>
		public IReadOnlyList<IActivity> GetActivities()
		{
			return ROActivities;
		}

		/// <summary>
		/// Точка расширения: возвращает начальное дочернее состояние, в которое нужно войти при Enter.
		/// По умолчанию null. Переопределять если состояние содержит начальное под-состояние.
		/// </summary>
		protected virtual BaseState GetInitialState()
		{
			return null;
		}

		/// <summary>
		/// Точка расширения: возвращает состояние для перехода из текущего состояния, если есть условие перехода.
		/// По умолчанию null. Переопределять для реализации логики переходов (transition guard).
		/// </summary>
		protected virtual BaseState GetTransition()
		{
			return null;
		}

		/// <summary>
		/// Вход в состояние: помечает текущим дочерним состояние в родителе и вызывает OnEnter.
		/// Также автоматически входит в начальное дочернее состояние, если оно задано.
		/// </summary>
		internal void Enter()
		{
			if (Parent != null)
			{
				Parent.CurrentChild = this;
			}

			OnEnter();

			var initialState = GetInitialState();
			if (initialState != null)
			{
				initialState.Enter();
			}
		}

		/// <summary>
		/// Выход из состояния: рекурсивно выходит из дочерних состояний, очищает CurrentChild и вызывает OnExit.
		/// </summary>
		internal void Exit()
		{
			if (CurrentChild != null)
			{
				CurrentChild.Exit();
			}

			CurrentChild = null;

			OnExit();
		}

		/// <summary>
		/// Обновление состояния: проверяет условие перехода (GetTransition), при его наличии делегирует
		/// планирование перехода последовательнику переходов машины. Если перехода нет — обновляет дочернее состояние
		/// или вызывает OnUpdate.
		/// </summary>
		internal void Update()
		{
			var transition = GetTransition();
			if (transition != null)
			{
				StateMachine.GetTransitionSequencer().RequestTransition(this, transition);
				return;
			}

			if (CurrentChild != null)
			{
				CurrentChild.Update();
			}

			OnUpdate();
		}
	}
}