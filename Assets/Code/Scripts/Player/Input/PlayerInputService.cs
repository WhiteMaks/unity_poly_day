using UnityEngine;

namespace Code.Scripts.Player.Input
{
	public class PlayerInputService : IPlayerInputService
	{
		private readonly InputSystem _inputSystem;

		private Vector2 _movement;

		public PlayerInputService()
		{
			_inputSystem = new InputSystem();
			_inputSystem.Enable();

			_inputSystem.Player.Movement.performed += ctx => _movement = ctx.ReadValue<Vector2>();
		}

		public Vector2 GetMovement()
		{
			return _movement;
		}
	}
}