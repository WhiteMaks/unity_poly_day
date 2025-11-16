using Code.Scripts.Camera.Input;
using Code.Scripts.Camera.Movement;
using Code.Scripts.Support;

namespace Code.Scripts.Camera
{
	public class CameraContext
	{
		private readonly ICameraInputService _inputService;
		private readonly CameraMovement _movement;
		private readonly CameraConfig _config;
		private readonly StateMachine _stateMachine;

		private IState _followState;
		private IState _rotateState;

		public CameraContext(ICameraInputService inputService, CameraMovement movement, CameraConfig config, StateMachine stateMachine)
		{
			_inputService = inputService;
			_movement = movement;
			_config = config;
			_stateMachine = stateMachine;
		}

		public ICameraInputService GetInputService()
		{
			return _inputService;
		}

		public CameraMovement GetMovement()
		{
			return _movement;
		}

		public CameraConfig GetConfig()
		{
			return _config;
		}

		public StateMachine GetStateMachine()
		{
			return _stateMachine;
		}

		public IState GetFollowState()
		{
			return _followState;
		}

		public void SetFollowState(IState followState)
		{
			_followState = followState;
		}

		public IState GetRotateState()
		{
			return _rotateState;
		}

		public void SetRotateState(IState rotateState)
		{
			_rotateState = rotateState;
		}
	}
}
