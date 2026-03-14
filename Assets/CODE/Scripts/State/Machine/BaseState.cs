using System.Collections.Generic;
using CODE.Scripts.State.Machine.Interfaces;

namespace CODE.Scripts.State.Machine
{
	public abstract class BaseState
	{
		private readonly List<IActivity> _activities = new();

		protected readonly StateMachine StateMachine;
		protected readonly BaseState Parent;

		protected BaseState CurrentChild;
		protected IReadOnlyList<IActivity> ROActivities => _activities;

		protected BaseState(StateMachine stateMachine, BaseState parent)
		{
			StateMachine = stateMachine;
			Parent = parent;
		}

		public void Add(IActivity activity)
		{
			if (activity != null)
			{
				_activities.Add(activity);
			}
		}

		protected virtual void OnEnter()
		{
		}

		protected virtual void OnExit()
		{
		}

		protected virtual void OnUpdate()
		{
		}

		protected virtual void OnLateUpdate()
		{
		}

		protected virtual void OnFixedUpdate()
		{
		}

		public BaseState GetDeepestChild()
		{
			var result = this;

			while (result.CurrentChild != null)
			{
				result = result.CurrentChild;
			}

			return result;
		}

		public IEnumerable<BaseState> GetPathToRoot()
		{
			var result = this;

			while (result != null)
			{
				yield return result;

				result = result.Parent;
			}
		}

		public BaseState GetParent()
		{
			return Parent;
		}

		public IReadOnlyList<IActivity> GetActivities()
		{
			return ROActivities;
		}

		protected virtual BaseState GetInitialState()
		{
			return null;
		}

		protected virtual BaseState GetTransition()
		{
			return null;
		}

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

		internal void Exit()
		{
			if (CurrentChild != null)
			{
				CurrentChild.Exit();
			}

			CurrentChild = null;

			OnExit();
		}

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