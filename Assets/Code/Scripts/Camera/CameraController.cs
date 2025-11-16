using Code.Scripts.Camera.Event;
using Code.Scripts.Camera.Input;
using Code.Scripts.Camera.Movement;
using Code.Scripts.Camera.State;
using Code.Scripts.Support;
using Code.Scripts.Wrappers;
using UnityEngine;

namespace Code.Scripts.Camera
{
	public class CameraController : BaseMonoBehaviour
	{
		private Transform _target;
		private CameraConfig _config;

		private CameraContext _context;

		public void Initialize(Transform target, CameraConfig config)
		{
			var inputService = new CameraInputService();
			var movement = new CameraMovement(transform, target, config);
			var stateMachine = new StateMachine();

			_context = new CameraContext(inputService, movement, config, stateMachine);

			_context.SetFollowState(new CameraFollowState(_context));

			stateMachine.ChangeState(_context.GetFollowState());
		}

		protected override void Awake()
		{
			EventBus.Publish(new CameraAwakenEvent(transform));
		}

		protected override void LateUpdate()
		{
			_context?.GetStateMachine().Update();
		}
	}
}