using UnityEngine;

namespace CODE.Scripts.Services
{
	public class Service1
	{
		public void Initialize(string message)
		{
			Debug.Log($"service 1 initialized with message: {message}");
		}
	}
}