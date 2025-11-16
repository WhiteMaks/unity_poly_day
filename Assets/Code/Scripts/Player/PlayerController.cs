using Code.Scripts.Camera;
using Code.Scripts.Player.Input;
using Code.Scripts.Player.Movement;
using Code.Scripts.Player.State;
using Code.Scripts.Support;
using Code.Scripts.Wrappers;
using UnityEngine;

namespace Code.Scripts.Player
{
	[RequireComponent(typeof(CharacterController))]
	public class PlayerController : BaseMonoBehaviour
	{
		[SerializeField] private PlayerConfig playerConfig;

		[SerializeField] private CameraController cameraController;
		[SerializeField] private CameraConfig cameraConfig;

		private PlayerContext _context;

		protected override void Awake()
		{
			var characterController = GetComponent<CharacterController>();

			var inputService = new PlayerInputService();
			var movement = new PlayerMovement(characterController, transform, playerConfig);
			var stateMachine = new StateMachine();

			_context = new PlayerContext(inputService, movement, playerConfig, stateMachine);

			_context.SetIdleState(new PlayerIdleState(_context));
			_context.SetWalkState(new PlayerWalkState(_context));

			stateMachine.ChangeState(_context.GetIdleState());

			cameraController.Initialize(transform, cameraConfig);
		}

		protected override void Update()
		{
			_context.GetStateMachine().Update();
		}
	}
}