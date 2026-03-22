using CODE.Scripts.Character.Events;
using CODE.Scripts.Core;
using CODE.Scripts.State.Machine;
using UnityEngine;

namespace CODE.Scripts.Character.States
{
	public class CharacterMoveState : CharacterBaseState
	{
		public CharacterMoveState(StateMachine stateMachine, BaseState parent, CharacterContext context, EventBus eventBus)
			: base(stateMachine, parent, context, eventBus)
		{
		}

		protected override void OnEnter()
		{
			EventBus.Publish(new CharacterMoveStartEvent());
		}

		protected override void OnExit()
		{
			EventBus.Publish(new CharacterMoveStopEvent());
		}

		protected override void OnUpdate()
		{
			var input = new Vector3(Context.moveInput.x, 0, Context.moveInput.y);
			input.Normalize();

			var target = input * Context.moveSpeed;

			var current = new Vector3(Context.velocity.x, 0, Context.velocity.z);
			current = Vector3.MoveTowards(current, target, Context.accel * Time.deltaTime);

			Context.velocity.x = current.x;
			Context.velocity.z = current.z;
		}

		protected override BaseState GetTransition()
		{
			return Context.moveInput.magnitude <= 0.01f ? ((CharacterGroundedState)Parent).IdleState : null;
		}
	}
}