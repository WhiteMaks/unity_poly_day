using UnityEngine;

namespace Code.Scripts.Providers
{
	public class AIInputProvider : MonoBehaviour, IInputProvider
	{

		public Vector2 GetPlayerMovement()
		{
			throw new System.NotImplementedException();
		}

		public Vector2 GetCameraMovement()
		{
			throw new System.NotImplementedException();
		}

		public float GetCameraRotation()
		{
			throw new System.NotImplementedException();
		}
	}
}