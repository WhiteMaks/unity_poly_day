using CODE.Scripts.Core;
using CODE.Scripts.State.Machine;

namespace CODE.Scripts.Character.States
{
	public class CharacterRootState : CharacterBaseState
	{
		public readonly CharacterGroundedState GroundedState;
		public readonly CharacterAirborneState AirborneState;

		public CharacterRootState(StateMachine stateMachine, CharacterContext context, EventBus eventBus)
			: base(stateMachine, null, context, eventBus)
		{
			GroundedState = new CharacterGroundedState(stateMachine, this, context, eventBus);
			AirborneState = new CharacterAirborneState(stateMachine, this, context, eventBus);
		}

		protected override BaseState GetInitialState()
		{
			return GroundedState;
		}
	}
}