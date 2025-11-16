using UnityEngine;

namespace Code.Scripts.Camera
{
	[CreateAssetMenu(menuName = "Config/Camera")]
	public class CameraConfig : ScriptableObject
	{
		public float xAngle = 55f;
		public float distance = 10f;
		public float lookAtOffset = 1;
		public float rotationSpeed = 90f;
		public float rotationSmoothTime = 0.15f;
	}
}
