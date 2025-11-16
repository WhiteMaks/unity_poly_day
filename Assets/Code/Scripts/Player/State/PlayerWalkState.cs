using UnityEngine;

namespace Code.Scripts.Player.State
{
	public class PlayerWalkState : PlayerBaseState
	{
		public PlayerWalkState(PlayerContext context) : base(context)
		{
		}

		public override void Enter()
		{
		}

		public override void Update()
		{
			var input = Context.GetInputService().GetMovement();

			if (input == Vector2.zero)
			{
				Context.GetStateMachine().ChangeState(Context.GetWalkState());
				return;
			}

			Context.GetMovement().Move(input);
		}

		public override void Exit()
		{
		}
	}
}