namespace Code.Scripts.Support
{
	public interface IState
	{
		void Enter();
		void Update();
		void Exit();
	}
}