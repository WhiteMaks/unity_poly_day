namespace Code.Scripts.Camera.Input
{
	public class CameraInputService : ICameraInputService
	{
		private readonly InputSystem _inputSystem;

		private float _rotation;

		public CameraInputService()
		{
			_inputSystem = new InputSystem();
			_inputSystem.Enable();

			_inputSystem.Camera.Rotation.performed += ctx => _rotation = ctx.ReadValue<float>();
			_inputSystem.Camera.Rotation.canceled += ctx => _rotation = ctx.ReadValue<float>();
		}

		public float GetRotation()
		{
			return _rotation;
		}
	}
}