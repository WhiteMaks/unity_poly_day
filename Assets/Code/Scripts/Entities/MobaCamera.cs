using Code.Scripts.Wrappers;
using UnityEngine;

namespace Code.Scripts.Entities
{
	public class MobaCamera : BaseMonoBehaviour
	{
		private static MobaCamera _instance;

		[SerializeField] private float xAngle;
		[SerializeField] private float distance;
		[SerializeField] private float lookAtOffset;
		[SerializeField] private float rotationSpeed;
		[SerializeField] private float rotationSmoothTime;

		private float _yAngle;
		private float _currentRotationSpeed;
		private float _rotationVelocity;

		public static MobaCamera GetInstance()
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

		public void Rotate(float rotation)
		{
			var targetRotationSpeed = rotation * rotationSpeed;

			_currentRotationSpeed = Mathf.SmoothDamp(
				_currentRotationSpeed,
				targetRotationSpeed,
				ref _rotationVelocity,
				rotationSmoothTime
			);

			_yAngle += _currentRotationSpeed * Time.deltaTime;
			_yAngle = Mathf.Repeat(_yAngle, 360f);
		}

		public void Follow(Transform target)
		{
			var rotation = Quaternion.Euler(xAngle, _yAngle, 0f);
			var offset = rotation * new Vector3(0f, 0f, -distance);

			transform.position = target.position + offset;
			transform.LookAt(target.position + Vector3.up * lookAtOffset);
		}
	}
}