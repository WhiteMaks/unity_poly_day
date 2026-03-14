using CODE.Scripts.Character;
using CODE.Scripts.State.Machine;
using CODE.Scripts.State.Machine.Activities;

namespace CODE.Scripts.Player.States
{
	public class PlayerGroundedState : PlayerBaseState
	{
		private readonly PlayerIdleState _idleState;
		private readonly PlayerMoveState _moveState;

		public PlayerGroundedState(StateMachine stateMachine, BaseState parent, CharacterContext context) : base(stateMachine, parent, context)
		{
			_idleState = new PlayerIdleState(stateMachine, this, context);
			_moveState = new PlayerMoveState(stateMachine, this, context);

			Add(new DelayActivationActivity(2.5f));
			Add(new DelayActivationActivity(2.5f));
		}

		public PlayerIdleState GetIdleState()
		{
			return _idleState;
		}

		public PlayerMoveState GetMoveState()
		{
			return _moveState;
		}

		protected override BaseState GetInitialState()
		{
			return _idleState;
		}

		protected override BaseState GetTransition()
		{
			if (_context.jump)
			{
				_context.jump = false;

				var velocity = _context.rigidbody.linearVelocity;
				velocity.y = _context.jumpSpeed;
				_context.rigidbody.linearVelocity = velocity;

				return ((PlayerRootState) Parent).GetAirborneState();
			}

			return _context.grounded ? null : ((PlayerRootState)Parent).GetAirborneState();
		}
	}
}