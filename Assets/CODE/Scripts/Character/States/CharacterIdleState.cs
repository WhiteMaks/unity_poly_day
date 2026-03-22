using CODE.Scripts.Character.Events;
using CODE.Scripts.Core;
using CODE.Scripts.State.Machine;
using UnityEngine;

namespace CODE.Scripts.Character.States
{
	public class CharacterIdleState : CharacterBaseState
	{
		public CharacterIdleState(StateMachine stateMachine, BaseState parent, CharacterContext context, EventBus eventBus)
			: base(stateMachine, parent, context, eventBus)
		{
		}

		protected override void OnEnter()
		{
			Context.velocity = Vector3.zero;

			EventBus.Publish(new CharacterIdleStartEvent());
		}

		protected override void OnExit()
		{
			EventBus.Publish(new CharacterIdleStopEvent());
		}

		protected override BaseState GetTransition()
		{
			return Context.moveInput.magnitude > 0.01f ? ((CharacterGroundedState)Parent).MoveState : null;
		}
	}
}