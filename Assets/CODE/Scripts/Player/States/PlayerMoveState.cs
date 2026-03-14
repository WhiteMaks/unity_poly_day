using CODE.Scripts.Character;
using CODE.Scripts.State.Machine;
using UnityEngine;

namespace CODE.Scripts.Player.States
{
	public class PlayerMoveState : PlayerBaseState
	{
		public PlayerMoveState(StateMachine stateMachine, BaseState parent, CharacterContext context) : base(stateMachine, parent, context)
		{
		}

		protected override void OnUpdate()
		{
			var target = _context.move.x * _context.moveSpeed;
			_context.velocity.x = Mathf.MoveTowards(_context.velocity.x, target, _context.accel * Time.deltaTime);
		}

		protected override BaseState GetTransition()
		{
			if (!_context.grounded)
			{
				return ((PlayerRootState)Parent).GetAirborneState();
			}

			return Mathf.Abs(_context.move.x) <= 0.01f ? ((PlayerGroundedState)Parent).GetIdleState() : null;
		}
	}
}