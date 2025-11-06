using UnityEngine;

namespace Code.Scripts.Wrappers
{
	public class BaseMonoBehaviour : MonoBehaviour
	{

		// Initialization Methods
		protected virtual void Awake()
		{
		}

		protected virtual void OnEnable()
		{
		}

		protected virtual void Reset()
		{
		}

		protected virtual void Start()
		{
		}

		//Physics Methods
		protected virtual void FixedUpdate()
		{
		}

		//Game Logic Methods
		protected virtual void Update()
		{
		}

		protected virtual void LateUpdate()
		{
		}

		//Destroy Methods
		protected virtual void OnDisable()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		//Application State Methods
		protected virtual void OnApplicationFocus(bool hasFocus)
		{
		}
	}
}