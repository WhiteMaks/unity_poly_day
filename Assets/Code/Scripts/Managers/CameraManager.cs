using Code.Scripts.Entities;
using Code.Scripts.Managers.Player;
using Code.Scripts.Wrappers;
using UnityEngine;

namespace Code.Scripts.Managers
{
	public class CameraManager : BaseMonoBehaviour
	{
		private static CameraManager _instance;

		[SerializeField] private Transform target;

		private PlayerInputManager _playerInputManager;
		private MobaCamera _mobaCamera;

		protected override void Start()
		{
			_playerInputManager = PlayerInputManager.GetInstance();
			_mobaCamera = MobaCamera.GetInstance();
		}

		protected override void LateUpdate()
		{
			_mobaCamera.Rotate(_playerInputManager.GetCameraRotation());
			_mobaCamera.Follow(target);
		}
	}
}
