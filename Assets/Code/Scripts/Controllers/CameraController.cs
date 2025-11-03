using Code.Scripts.Providers;
using UnityEngine;

namespace Code.Scripts.Controllers
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Transform target;

		[SerializeField] private float xAngle;
		[SerializeField] private float distance;
		[SerializeField] private float lookAtOffset;
		[SerializeField] private float rotationSpeed;
		[SerializeField] private float rotationSmoothTime;

		private IInputProvider _inputProvider;

		private float _yAngle;
		private float _currentRotationSpeed;
		private float _rotationVelocity;

		private void Awake()
		{
			_inputProvider = GetComponent<IInputProvider>();
		}

		private void Update()
		{
			UpdateRotationByInput();
		}

		private void LateUpdate()
		{
			UpdatePosition();
			UpdateView();
		}

		private void UpdateRotationByInput()
		{
			var rotationInput = _inputProvider.GetCameraRotation();

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