using Code.Scripts.Managers.Character;
using UnityEngine;

namespace Code.Scripts.Managers.Player
{
	public class PlayerLocomotionManager : CharacterLocomotionManager
	{
		[SerializeField] private float movementSpeed;
		[SerializeField] private float rotationSpeed;

		private PlayerManager _playerManager;
		private CameraManager _cameraManager;
		private PlayerInputManager _playerInputManager;

		private Vector3 _moveDirection;
		private Vector3 _rotationDirection;

		private float _horizontalMovement;
		private float _verticalMovement;

		public void HandleAllMovement()
		{
			HandleInputMovement();
			HandleGroundedMovement();
			HandleViewRotation();
		}

		protected override void Awake()
		{
			base.Awake();

			_playerManager = GetComponent<PlayerManager>();
		}

		protected override void Start()
		{
			_cameraManager = CameraManager.GetInstance();
			_playerInputManager = PlayerInputManager.GetInstance();
		}

		private void HandleGroundedMovement()
		{
			_moveDirection = _cameraManager.transform.forward * _verticalMovement;
			_moveDirection += _cameraManager.transform.right * _horizontalMovement;

			_moveDirection.Normalize();
			_moveDirection.y = 0;

			_playerManager.GetCharacterController().Move(_moveDirection * (movementSpeed * Time.deltaTime));
		}

		private void HandleInputMovement()
		{
			var movement = _playerInputManager.GetPlayerMovement();

			_horizontalMovement = movement.x;
			_verticalMovement = movement.y;
		}

		private void HandleViewRotation()
		{
			_rotationDirection = _cameraManager.transform.forward * _verticalMovement;
			_rotationDirection += _cameraManager.transform.right * _horizontalMovement;

			_rotationDirection.Normalize();
			_rotationDirection.y = 0;

			if (_rotationDirection == Vector3.zero)
			{
				_rotationDirection = transform.forward;
			}

			var targetRotation = Quaternion.LookRotation(_rotationDirection);
			var newRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

			transform.rotation = newRotation;
		}
	}
}
