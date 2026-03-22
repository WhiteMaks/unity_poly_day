using CODE.Scripts.Core;
using CODE.Scripts.State.Machine;
using CODE.Scripts.State.Machine.Activities;

namespace CODE.Scripts.Character.States
{
	public class CharacterAirborneState : CharacterBaseState
	{
		public readonly CharacterJumpState JumpState;

		public CharacterAirborneState(StateMachine stateMachine, BaseState parent, CharacterContext context, EventBus eventBus)
			: base(stateMachine, parent, context, eventBus)
		{
			JumpState = new CharacterJumpState(stateMachine, this, context, eventBus);

			Add(new DelayActivationActivity(1f));
		}

		protected override BaseState GetInitialState()
		{
			if (Context.jumpInput)
			{
				Context.jumpInput = false;
				return JumpState;
			}

			return null;
		}

		protected override BaseState GetTransition()
		{
			return Context.grounded ? ((CharacterRootState)Parent).GroundedState : null;
		}

		protected override void OnEnter()
		{

		}
	}
}