namespace Code.Scripts.Camera.State
{
	public class CameraFollowState : CameraBaseState
	{
		public CameraFollowState(CameraContext context) : base(context)
		{
		}

		public override void Enter()
		{

		}

		public override void Update()
		{
			var input = Context.GetInputService().GetRotation();

			Context.GetMovement().Follow();
			Context.GetMovement().Rotate(input);
		}

		public override void Exit()
		{

		}
	}
}
