using CODE.Scripts.Character;
using CODE.Scripts.State.Machine;
using UnityEngine;

namespace CODE.Scripts.Player.States
{
	public class PlayerIdleState : PlayerBaseState
	{
		public PlayerIdleState(StateMachine stateMachine, BaseState parent, CharacterContext context) : base(stateMachine, parent, context)
		{
		}

		protected override void OnEnter()
		{
			_context.velocity = Vector3.zero;
		}

		protected override BaseState GetTransition()
		{
			return Mathf.Abs(_context.move.x) > 0.01f ? ((PlayerGroundedState)Parent).GetMoveState() : null;
		}
	}
}