using Code.Scripts.Player.Input;
using Code.Scripts.Player.Movement;
using Code.Scripts.Support;

namespace Code.Scripts.Player
{
	public class PlayerContext
	{
		private readonly IPlayerInputService _inputService;
		private readonly PlayerMovement _movement;
		private readonly PlayerConfig _config;
		private readonly StateMachine _stateMachine;

		private IState _idleState;
		private IState _walkState;

		public PlayerContext(IPlayerInputService inputService, PlayerMovement movement, PlayerConfig config, StateMachine stateMachine)
		{
			_inputService = inputService;
			_movement = movement;
			_config = config;
			_stateMachine = stateMachine;
		}

		public IPlayerInputService GetInputService()
		{
			return _inputService;
		}

		public PlayerMovement GetMovement()
		{
			return _movement;
		}

		public PlayerConfig GetConfig()
		{
			return _config;
		}

		public StateMachine GetStateMachine()
		{
			return _stateMachine;
		}

		public IState GetIdleState()
		{
			return _idleState;
		}

		public void SetIdleState(IState idleState)
		{
			_idleState = idleState;
		}

		public IState GetWalkState()
		{
			return _walkState;
		}

		public void SetWalkState(IState walkState)
		{
			_walkState = walkState;
		}
	}
}
