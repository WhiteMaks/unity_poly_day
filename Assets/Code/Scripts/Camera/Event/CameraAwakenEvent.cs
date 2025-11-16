using UnityEngine;

namespace Code.Scripts.Camera.Event
{
	public struct CameraAwakenEvent
	{
		public readonly Transform Transform;

		public CameraAwakenEvent(Transform transform)
		{
			Transform = transform;
		}
	}
}
