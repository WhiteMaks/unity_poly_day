using System;
using CODE.Scripts.Character.Events;
using CODE.Scripts.Core;
using UnityEngine;

namespace CODE.Scripts.Character.Systems
{
	public class CharacterPhysicsSystem
	{
		private readonly CharacterView _view;
		private readonly CharacterContext _context;

		public CharacterPhysicsSystem(EventBus eventBus, CharacterContext context, CharacterView view)
		{
			_context = context;
			_view = view;

			eventBus.Subscribe<CharacterJumpStartEvent>(OnJumpStartEvent);
		}

		public void FixedUpdate()
		{
			var velocity = _view.rigidbody.linearVelocity;
			velocity.x = _context.velocity.x;
			velocity.z = _context.velocity.z;

			_view.rigidbody.linearVelocity = velocity;

			_context.velocity = _view.rigidbody.linearVelocity;
			_context.grounded = CheckGround();
		}

		private void OnJumpStartEvent(CharacterJumpStartEvent jumpEvent)
		{
			var velocity = _view.rigidbody.linearVelocity;
			velocity.y = _context.velocity.y;

			_view.rigidbody.linearVelocity = velocity;

			_context.velocity.y = 0;
		}

		private bool CheckGround()
		{
			return Physics.CheckSphere(_view.transform.position, _view.sphereGroundCheckRadius, _view.groundMask);
		}
	}
}