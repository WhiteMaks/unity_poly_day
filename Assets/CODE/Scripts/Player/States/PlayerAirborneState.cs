using CODE.Scripts.Character;
using CODE.Scripts.State.Machine;

namespace CODE.Scripts.Player.States
{
	public class PlayerAirborneState : PlayerBaseState
	{

		public PlayerAirborneState(StateMachine stateMachine, BaseState parent, CharacterContext context) : base(stateMachine, parent, context)
		{
		}

		protected override BaseState GetTransition()
		{
			return _context.grounded ? ((PlayerRootState)Parent).GetGroundedState() : null;
		}

		protected override void OnEnter()
		{

		}
	}
}