using System;
using UnityEngine;

namespace CODE.Scripts.Source.Interfaces
{
	public interface IInputSource
	{
		event Action<Vector2> OnMove;
		event Action OnJump;

		void Enable();
		void Disable();
	}
}