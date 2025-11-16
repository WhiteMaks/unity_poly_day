using Code.Scripts.Camera.Event;
using Code.Scripts.Support;
using UnityEngine;

namespace Code.Scripts.Player.Movement
{
	public class PlayerMovement
	{
		private readonly CharacterController _controller;
		private readonly Transform _transform;
		private readonly PlayerConfig _config;

		private Transform _cameraTransform;

		public PlayerMovement(CharacterController controller, Transform transform, PlayerConfig config)
		{
			_controller = controller;
			_transform = transform;
			_config = config;

			EventBus.Subscribe<CameraAwakenEvent>(OnCameraAwaken);
		}

		public void Move(Vector2 input)
		{
			var vertical = input.y;
			var horizontal = input.x;

			var moveDirection = _cameraTransform.forward * vertical + _cameraTransform.right * horizontal;
			moveDirection.y = 0;
			moveDirection.Normalize();

			_controller.Move(moveDirection * (_config.movementSpeed * Time.deltaTime));

			if (moveDirection == Vector3.zero)
			{
				moveDirection = _transform.forward;
			}

			var targetRotation = Quaternion.LookRotation(moveDirection);
			var newRotation = Quaternion.Slerp(_transform.rotation, targetRotation, _config.rotationSpeed * Time.deltaTime);

			_transform.rotation = newRotation;
		}

		private void OnCameraAwaken(CameraAwakenEvent evt)
		{
			_cameraTransform = evt.Transform;
		}
	}
}