using Code.Scripts.Wrappers;
using UnityEngine;

namespace Code.Scripts.Managers.Player
{
	public class PlayerInputManager : BaseMonoBehaviour
	{
		private static PlayerInputManager _instance;

		private InputSystem _inputSystem;

		private Vector2 _playerMovement;

		private float _cameraRotation;

		public static PlayerInputManager GetInstance()
		{
			return _instance;
		}

		protected override void OnEnable()
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

		protected override void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
				return;
			}

			Destroy(gameObject);
		}

		protected override void OnDisable()
		{
			_inputSystem.Disable();
		}

		protected override void OnApplicationFocus(bool hasFocus)
		{
			if (!enabled)
			{
				return;
			}

			if (hasFocus)
			{
				_inputSystem.Enable();
			}
			else
			{
				_inputSystem.Disable();
			}
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