using CODE.Scripts.Core;
using CODE.Scripts.State.Machine;

namespace CODE.Scripts.Character.States
{
	public class CharacterGroundedState : CharacterBaseState
	{
		public readonly CharacterIdleState IdleState;
		public readonly CharacterMoveState MoveState;

		public CharacterGroundedState(StateMachine stateMachine, BaseState parent, CharacterContext context, EventBus eventBus)
			: base(stateMachine, parent, context, eventBus)
		{
			IdleState = new CharacterIdleState(stateMachine, this, context, eventBus);
			MoveState = new CharacterMoveState(stateMachine, this, context, eventBus);
		}

		protected override BaseState GetInitialState()
		{
			if (Context.moveInput.magnitude > 0.01f)
			{
				return MoveState;
			}

			return IdleState;
		}

		protected override BaseState GetTransition()
		{
			if (!Context.grounded || Context.jumpInput)
			{
				return ((CharacterRootState)Parent).AirborneState;
			}

			return null;
		}
	}
}