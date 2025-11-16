using UnityEngine;

namespace Code.Scripts.Player
{
	[CreateAssetMenu(menuName = "Config/Player")]
	public class PlayerConfig : ScriptableObject
	{
		public float movementSpeed = 5f;
		public float rotationSpeed = 5f;
	}
}