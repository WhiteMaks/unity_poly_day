using Code.Scripts.Entities;
using Code.Scripts.Managers.Character;
using UnityEngine;

namespace Code.Scripts.Managers.Player
{
	public class PlayerLocomotionManager : CharacterLocomotionManager
	{
		[SerializeField] private float movementSpeed;
		[SerializeField] private float rotationSpeed;

		private PlayerManager _playerManager;
		private PlayerInputManager _playerInputManager;
		private MobaCamera _mobaCamera;

		private Vector3 _moveDirection;
		private Vector3 _rotationDirection;

		private float _horizontalMovement;
		private float _verticalMovement;

		public void HandleMovement()
		{
			HandleInput();

			UpdatePosition();
			UpdateRotation();
		}

		protected override void Awake()
		{
			base.Awake();

			_playerManager = GetComponent<PlayerManager>();
		}

		protected override void Start()
		{
			_playerInputManager = PlayerInputManager.GetInstance();
			_mobaCamera = MobaCamera.GetInstance();
		}

		private void UpdatePosition()
		{
			_moveDirection = _mobaCamera.transform.forward * _verticalMovement;
			_moveDirection += _mobaCamera.transform.right * _horizontalMovement;

			_moveDirection.Normalize();
			_moveDirection.y = 0;

			_playerManager.GetCharacterController().Move(_moveDirection * (movementSpeed * Time.deltaTime));
		}

		private void HandleInput()
		{
			var movement = _playerInputManager.GetPlayerMovement();

			_horizontalMovement = movement.x;
			_verticalMovement = movement.y;
		}

		private void UpdateRotation()
		{
			_rotationDirection = _mobaCamera.transform.forward * _verticalMovement;
			_rotationDirection += _mobaCamera.transform.right * _horizontalMovement;

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
