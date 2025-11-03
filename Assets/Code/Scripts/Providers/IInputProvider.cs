using UnityEngine;

namespace Code.Scripts.Providers
{
	public interface IInputProvider
	{
		Vector2 GetPlayerMovement();

		float GetCameraRotation();
	}
}