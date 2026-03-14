using CODE.Scripts.Character;
using CODE.Scripts.State.Machine;

namespace CODE.Scripts.Player.States
{
	public class PlayerRootState : PlayerBaseState
	{
		private readonly PlayerGroundedState _groundedState;
		private readonly PlayerAirborneState _airborneState;

		public PlayerRootState(StateMachine stateMachine, CharacterContext context) : base(stateMachine, null, context)
		{
			_groundedState = new PlayerGroundedState(stateMachine, this, context);
			_airborneState = new PlayerAirborneState(stateMachine, this, context);
		}

		public PlayerGroundedState GetGroundedState()
		{
			return _groundedState;
		}

		public PlayerAirborneState GetAirborneState()
		{
			return _airborneState;
		}

		protected override BaseState GetInitialState()
		{
			return _groundedState;
		}

		protected override BaseState GetTransition()
		{
			return _context.grounded ? null : _airborneState;
		}
	}
}