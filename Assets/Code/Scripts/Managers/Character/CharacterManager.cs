using Code.Scripts.Wrappers;
using UnityEngine;

namespace Code.Scripts.Managers.Character
{
	public class CharacterManager : BaseMonoBehaviour
	{
		private CharacterController _characterController;

		protected override void Awake()
		{
			_characterController = GetComponent<CharacterController>();
		}

		public CharacterController GetCharacterController()
		{
			return _characterController;
		}
	}
}
