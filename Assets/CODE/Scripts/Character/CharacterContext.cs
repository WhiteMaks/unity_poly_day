using System;
using UnityEngine;

namespace CODE.Scripts.Character
{
	[Serializable]
	public class CharacterContext
	{
		public Vector3 moveInput;
		public bool jumpInput;

		public Vector3 velocity;

		public float jumpSpeed = 7f;
		public float moveSpeed = 6f;
		public float accel = 40f;

		public bool grounded;
	}
}