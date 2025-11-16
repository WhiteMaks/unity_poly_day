using UnityEngine;

namespace Code.Scripts.Camera.Movement
{
	public class CameraMovement
	{
		private readonly Transform _transform;
		private readonly Transform _target;
		private readonly CameraConfig _config;

		private float _yAngle;
		private float _currentRotationSpeed;
		private float _rotationVelocity;

		public CameraMovement(Transform transform, Transform target, CameraConfig config)
		{
			_transform = transform;
			_target = target;
			_config = config;
		}

		public void Follow()
		{
			var rotation = Quaternion.Euler(_config.xAngle, _yAngle, 0f);
			var offset = rotation * new Vector3(0f, 0f, -_config.distance);

			_transform.position = _target.position + offset;
			_transform.LookAt(_target.position + Vector3.up * _config.lookAtOffset);
		}

		public void Rotate(float rotation)
		{
			var targetRotationSpeed = rotation * _config.rotationSpeed;

			_currentRotationSpeed = Mathf.SmoothDamp(
				_currentRotationSpeed,
				targetRotationSpeed,
				ref _rotationVelocity,
				_config.rotationSmoothTime
			);

			_yAngle += _currentRotationSpeed * Time.deltaTime;
			_yAngle = Mathf.Repeat(_yAngle, 360f);
		}
	}
}