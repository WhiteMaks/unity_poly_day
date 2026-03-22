using System;
using UnityEngine;

namespace CODE.Scripts.Character
{
	[Serializable]
	public class CharacterView
	{
		public Transform transform;
		public Rigidbody rigidbody;
		public Animator animator;
		public LayerMask groundMask;
		public float sphereGroundCheckRadius;
	}
}