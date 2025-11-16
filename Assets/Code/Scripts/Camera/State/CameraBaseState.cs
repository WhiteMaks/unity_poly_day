using Code.Scripts.Support;

namespace Code.Scripts.Camera.State
{
	public abstract class CameraBaseState : IState
	{
		protected readonly CameraContext Context;

		protected CameraBaseState(CameraContext context)
		{
			Context = context;
		}

		public abstract void Enter();
		public abstract void Update();
		public abstract void Exit();
	}
}