using UnityEngine;

namespace Code.Scripts.Providers
{
	public class PlayerInputProvider : MonoBehaviour, IInputProvider
	{
		private InputSystem _inputSystem;

		private Vector2 _playerMovement;

		private float _cameraRotation;

		private void OnEnable()
		{
			if (_inputSystem == null)
			{
				_inputSystem = new InputSystem();

				_inputSystem.PlayerMovement.Movement.performed += ctx => _playerMovement = ctx.ReadValue<Vector2>();

				_inputSystem.CameraMovement.Rotation.performed += ctx => _cameraRotation = ctx.ReadValue<float>();
				_inputSystem.CameraMovement.Rotation.canceled += ctx => _cameraRotation = ctx.ReadValue<float>();
			}

			_inputSystem.Enable();
		}

		private void OnDisable()
		{
			_inputSystem.Disable();
		}

		public Vector2 GetPlayerMovement()
		{
			return _playerMovement;
		}

		public float GetCameraRotation()
		{
			return _cameraRotation;
		}
	}
}