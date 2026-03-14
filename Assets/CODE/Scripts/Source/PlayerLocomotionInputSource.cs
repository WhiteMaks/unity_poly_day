using System;
using CODE.Scripts.Source.Interfaces;
using UnityEngine;

namespace CODE.Scripts.Source
{
	public class PlayerLocomotionInputSource : IInputSource
	{
		private readonly IA_PlayerLocomotion _playerLocomotion;

		public event Action<Vector2> OnMove;
		public event Action OnJump;

		public PlayerLocomotionInputSource()
		{
			_playerLocomotion = new IA_PlayerLocomotion();
			_playerLocomotion.Movement.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
			_playerLocomotion.Movement.Move.canceled += ctx => OnMove?.Invoke(Vector2.zero);
			_playerLocomotion.Movement.Jump.performed += ctx => OnJump?.Invoke();
		}

		public void Enable()
		{
			_playerLocomotion.Enable();
		}

		public void Disable()
		{
			_playerLocomotion.Disable();
		}

	}
}