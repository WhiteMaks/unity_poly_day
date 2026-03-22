using CODE.Scripts.Character.Events;
using CODE.Scripts.Core;
using CODE.Scripts.State.Machine;

namespace CODE.Scripts.Character.States
{
	public class CharacterJumpState : CharacterBaseState
	{
		public CharacterJumpState(StateMachine stateMachine, BaseState parent, CharacterContext context, EventBus eventBus)
			: base(stateMachine, parent, context, eventBus)
		{
		}

		protected override void OnEnter()
		{
			var velocity = Context.velocity;
			velocity.y = Context.jumpSpeed;

			Context.velocity = velocity;

			EventBus.Publish(new CharacterJumpStartEvent());
		}

		protected override void OnExit()
		{
			Context.jumpInput = false;

			EventBus.Publish(new CharacterJumpStopEvent());
		}
	}

}