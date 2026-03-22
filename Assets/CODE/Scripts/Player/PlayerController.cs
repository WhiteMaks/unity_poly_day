using System.Linq;
using CODE.Scripts.Character;
using CODE.Scripts.Character.States;
using CODE.Scripts.Character.Systems;
using CODE.Scripts.Core;
using CODE.Scripts.Observer;
using CODE.Scripts.Observer.Interfaces;
using CODE.Scripts.Source;
using CODE.Scripts.Source.Interfaces;
using CODE.Scripts.State.Machine;
using UnityEngine;

namespace CODE.Scripts.Player
{
	public class PlayerController : MonoBehaviour, IStartObserver, IUpdateObserver, IFixedUpdateObserver
	{
		private IInputSource _inputSource;
		private EventBus _eventBus;
		private StateMachine _stateMachine;
		private BaseState _rootSate;
		private CharacterPhysicsSystem _physicsSystem;

		private string _lastPath;

		[SerializeField] private CharacterContext context;
		[SerializeField] private CharacterView view;

		public void CoreStart()
		{
			_inputSource = new PlayerControllerInputSource();
			_inputSource.Enable();

			_inputSource.OnMove += OnMoveInput;
			_inputSource.OnJump += OnJumpInput;

			_eventBus = new EventBus();

			_rootSate = new CharacterRootState(_stateMachine, context, _eventBus);
			_stateMachine = new StateMachineBuilder(_rootSate).Build();

			_physicsSystem = new CharacterPhysicsSystem(_eventBus, context, view);

			view.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		}

		public void CoreUpdate()
		{
			_stateMachine.Update();
			var path = StatePath(_stateMachine.GetRootState().GetDeepestChild());
			if (path != _lastPath)
			{
				Debug.Log($"Current State: {path}");
				_lastPath = path;
			}
		}

		public void CoreFixedUpdate()
		{
			_physicsSystem.FixedUpdate();
		}

		private static string StatePath(BaseState state)
		{
			return string.Join(" > ", state.GetPathToRoot().Reverse().Select(s => s.GetType().Name));
		}

		private void OnMoveInput(Vector2 input)
		{
			context.moveInput.x = Mathf.Clamp(input.x, -1.0f, 1.0f);
			context.moveInput.y = Mathf.Clamp(input.y, -1.0f, 1.0f);
		}

		private void OnJumpInput()
		{
			context.jumpInput = true;
		}

		private void Awake()
		{
			StartSceneManager.Add(this);
			UpdateSceneManager.Add(this);
			FixedUpdateSceneManager.Add(this);
		}

		private void OnDisable()
		{
			_inputSource?.Disable();
		}
	}
}