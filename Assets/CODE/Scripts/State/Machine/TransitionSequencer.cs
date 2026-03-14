using System;
using System.Collections.Generic;
using System.Threading;
using CODE.Scripts.State.Machine.Enums;
using CODE.Scripts.State.Machine.Interfaces;
using CODE.Scripts.State.Machine.Sequences;

namespace CODE.Scripts.State.Machine
{
	public class TransitionSequencer
	{
		private readonly StateMachine _stateMachine;

		private readonly bool _useSequential = false;

		private ISequence _currentSequence;
		private Action _nextPhase;

		private BaseState _pendingFromState;
		private BaseState _pendingToState;

		private BaseState _lastFromState;
		private BaseState _lastToState;

		private CancellationTokenSource _tokenSource;

		public TransitionSequencer(StateMachine stateMachine)
		{
			_stateMachine = stateMachine;
		}

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

		private static List<BaseState> GetStatesToExit(BaseState fromState, BaseState toState)
		{
			var result = new List<BaseState>();

			for (var state = fromState; state != null && state != toState; state = state.GetParent()) {
				result.Add(state);
			}

			return result;
		}

		private static List<BaseState> GetStatesToEnter(BaseState fromState, BaseState toState)
		{
			var result = new Stack<BaseState>();

			for (var state = fromState; state != null && state != toState; state = state.GetParent()) {
				result.Push(state);
			}

			return new List<BaseState>(result);
		}

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

		private ISequence GetPhaseSequence(List<PhaseStep> steps, CancellationToken token)
		{
			return _useSequential ? new SequentialPhaseSequence(steps, token) : new ParallelPhaseSequence(steps, token);
		}
	}
}