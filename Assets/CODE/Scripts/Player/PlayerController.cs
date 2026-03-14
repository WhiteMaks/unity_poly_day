using System.Linq;
using CODE.Scripts.Character;
using CODE.Scripts.Dependency.Injection.Attributes;
using CODE.Scripts.Observer;
using CODE.Scripts.Observer.Interfaces;
using CODE.Scripts.Player.States;
using CODE.Scripts.Services;
using CODE.Scripts.Source;
using CODE.Scripts.Source.Interfaces;
using CODE.Scripts.State.Machine;
using UnityEngine;

namespace CODE.Scripts.Player
{
	public class PlayerController : MonoBehaviour, IStartObserver, IUpdateObserver, IFixedUpdateObserver
	{
		private IInputSource _inputSource;
		private StateMachine _stateMachine;
		private BaseState _rootSate;

		[SerializeField] private CharacterContext context;
		[SerializeField] private LayerMask groundMask;
		[SerializeField] private Transform groundCheck;
		private Rigidbody _rigidbody;

		private const float GroundRadius = 0.6f;
		private const bool DrawGizmos = true;

		private string _lastPath;

		[Inject] private Service1 _service1;

		public void CoreStart()
		{
			_service1.Initialize("Service 1 initialized in PlayerController");

			_inputSource = new PlayerLocomotionInputSource();
			_inputSource.Enable();

			_inputSource.OnMove += OnMoveInput;
			_inputSource.OnJump += OnJumpInput;

			_rigidbody = gameObject.GetComponent<Rigidbody>();
			_rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

			context.rigidbody = _rigidbody;
			context.animator = GetComponentInChildren<Animator>();

			_rootSate = new PlayerRootState(_stateMachine, context);
			_stateMachine = new StateMachineBuilder(_rootSate).Build();
		}

		public void CoreUpdate()
		{
			context.grounded = Physics.CheckSphere(groundCheck.position, GroundRadius, groundMask);

			_stateMachine.Update();

			var path = StatePath(_stateMachine.GetRootState().GetDeepestChild());
			if (path != _lastPath)
			{
				Debug.Log(path);
				_lastPath = path;
			}
		}

		public void CoreFixedUpdate()
		{
			var velocity = _rigidbody.linearVelocity;
			velocity.x = context.velocity.x;

			_rigidbody.linearVelocity = velocity;
			context.velocity.x = _rigidbody.linearVelocity.x;
		}

		private static string StatePath(BaseState state)
		{
			return string.Join(" > ", state.GetPathToRoot().Reverse().Select(s => s.GetType().Name));
		}

		private void OnDrawGizmosSelected()
		{
			if (!DrawGizmos || groundCheck == null)
			{
				return;
			}

			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(groundCheck.position, GroundRadius);
		}

		private void OnMoveInput(Vector2 input)
		{
			context.move.x = Mathf.Clamp(input.x, -1.0f, 1.0f);
		}

		private void OnJumpInput()
		{
			context.jump = true;
		}

		private void Awake()
		{
			StartSceneManager.Add(this);
			UpdateSceneManager.Add(this);
			FixedUpdateSceneManager.Add(this);
		}

		private void OnDisable()
		{
			_inputSource.Disable();
		}
	}
}