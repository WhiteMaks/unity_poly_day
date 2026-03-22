using System;
using CODE.Scripts.Source.Interfaces;
using UnityEngine;

namespace CODE.Scripts.Source
{
	public class PlayerControllerInputSource : IInputSource
	{
		private readonly IA_PlayerController _playerController;

		public event Action<Vector2> OnMove;
		public event Action OnJump;

		public PlayerControllerInputSource()
		{
			_playerController = new IA_PlayerController();
			_playerController.Controller.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
			_playerController.Controller.Move.canceled += ctx => OnMove?.Invoke(Vector2.zero);
			_playerController.Controller.Jump.performed += ctx => OnJump?.Invoke();
		}

		public void Enable()
		{
			_playerController.Enable();
		}

		public void Disable()
		{
			_playerController.Disable();
		}

	}
}