using Code.Scripts.Managers.Player;
using Code.Scripts.Wrappers;
using UnityEngine;

namespace Code.Scripts.Managers
{
	public class CameraManager : BaseMonoBehaviour
	{
		private static CameraManager _instance;

		[SerializeField] private Transform target;

		[SerializeField] private float xAngle;
		[SerializeField] private float distance;
		[SerializeField] private float lookAtOffset;
		[SerializeField] private float rotationSpeed;
		[SerializeField] private float rotationSmoothTime;

		private PlayerInputManager _playerInputManager;

		private float _yAngle;
		private float _currentRotationSpeed;
		private float _rotationVelocity;

		public static CameraManager GetInstance()
		{
			return _instance;
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

		protected override void Start()
		{
			_playerInputManager = PlayerInputManager.GetInstance();
		}

		protected override void Update()
		{
			UpdateRotationByInput();
		}

		protected override void LateUpdate()
		{
			UpdatePosition();
			UpdateView();
		}

		private void UpdateRotationByInput()
		{
			var rotationInput = _playerInputManager.GetCameraRotation();

			var targetRotationSpeed = rotationInput * rotationSpeed;

			_currentRotationSpeed = Mathf.SmoothDamp(
				_currentRotationSpeed,
				targetRotationSpeed,
				ref _rotationVelocity,
				rotationSmoothTime
			);

			_yAngle += _currentRotationSpeed * Time.deltaTime;
			_yAngle = Mathf.Repeat(_yAngle, 360f);
		}

		private void UpdatePosition()
		{
			var rotation = Quaternion.Euler(xAngle, _yAngle, 0f);

			var offset = rotation * new Vector3(0f, 0f, -distance);

			transform.position = target.position + offset;
		}

		private void UpdateView()
		{
			transform.LookAt(target.position + Vector3.up * lookAtOffset);
		}
	}
}
