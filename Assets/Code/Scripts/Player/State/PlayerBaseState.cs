using Code.Scripts.Support;

namespace Code.Scripts.Player.State
{
	public abstract class PlayerBaseState : IState
	{
		protected readonly PlayerContext Context;

		protected PlayerBaseState(PlayerContext context)
		{
			Context = context;
		}

		public abstract void Enter();
		public abstract void Update();
		public abstract void Exit();
	}
}
