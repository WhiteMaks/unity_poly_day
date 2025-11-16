using UnityEngine;

namespace Code.Scripts.Player.State
{
	public class PlayerIdleState : PlayerBaseState
	{
		public PlayerIdleState(PlayerContext context) : base(context)
		{
		}

		public override void Enter()
		{
		}

		public override void Update()
		{
			if (Context.GetInputService().GetMovement() != Vector2.zero)
			{
				Context.GetStateMachine().ChangeState(Context.GetWalkState());
			}
		}

		public override void Exit()
		{
		}
	}
}