using System.Collections.Generic;

namespace CODE.Scripts.State.Machine
{
	public class StateMachine
	{
		private readonly BaseState _rootState;
		private readonly TransitionSequencer _transitionSequencer;

		private bool _started;

		public StateMachine(BaseState rootState)
		{
			_rootState = rootState;
			_transitionSequencer = new TransitionSequencer(this);
		}

		public void Start()
		{
			if (_started)
			{
				return;
			}

			_started = true;

			_rootState.Enter();
		}

		public void Update()
		{
			if (!_started)
			{
				Start();
			}

			_transitionSequencer.Update();
		}

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

		public TransitionSequencer GetTransitionSequencer()
		{
			return _transitionSequencer;
		}

		public BaseState GetRootState()
		{
			return _rootState;
		}

		internal void InternalUpdate()
		{
			_rootState.Update();
		}
	}
}