using System;
using UnityEngine;

namespace CODE.Scripts.Character
{
	[Serializable]
	public class CharacterContext
	{
		public Animator animator;
		public Rigidbody rigidbody;

		public Vector3 move;
		public Vector3 velocity;

		public bool grounded;
		public bool jump;

		public float moveSpeed = 6f;
		public float accel = 40f;
		public float jumpSpeed = 7f;
	}
}