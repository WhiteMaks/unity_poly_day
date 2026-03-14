using System.Collections.Generic;
using System.Reflection;

namespace CODE.Scripts.State.Machine
{
	public class StateMachineBuilder
	{
		private readonly BaseState _rootState;

		public StateMachineBuilder(BaseState rootState)
		{
			_rootState = rootState;
		}

		public StateMachine Build()
		{
			var result = new StateMachine(_rootState);

			Wire(_rootState, result, new HashSet<BaseState>());

			return result;
		}

		private static void Wire(BaseState state, StateMachine stateMachine, HashSet<BaseState> visited)
		{
			if (state == null)
			{
				return;
			}

			if (!visited.Add(state))
			{
				return;
			}

			const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

			var stateMachineManagerField = state.GetType().GetField("StateMachine", flags);
			if (stateMachineManagerField != null)
			{
				stateMachineManagerField.SetValue(state, stateMachine);
			}

			foreach (var field in state.GetType().GetFields(flags))
			{
				if (!typeof(BaseState).IsAssignableFrom(field.FieldType))
				{
					continue;
				}

				if (field.Name == "Parent")
				{
					continue;
				}

				var childState = (BaseState) field.GetValue(state);
				if (childState == null)
				{
					continue;
				}

				if (!ReferenceEquals(childState.GetParent(), state))
				{
					continue;
				}

				Wire(childState, stateMachine, visited);
			}
		}
	}
}