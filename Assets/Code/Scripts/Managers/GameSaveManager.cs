using System.Collections;
using Code.Scripts.Wrappers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Managers
{
	public class GameSaveManager : BaseMonoBehaviour
	{
		private static GameSaveManager _instance;

		[SerializeField] private byte worldSceneIndex;

		public static GameSaveManager GetInstance()
		{
			return _instance;
		}

		protected override void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
				return;
			}

			Destroy(gameObject);
		}

		protected override void Start()
		{
			DontDestroyOnLoad(gameObject);
		}

		public IEnumerator StartNewGame()
		{
			var loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

			yield return null;
		}

		public byte GetWorldSceneIndex()
		{
			return worldSceneIndex;
		}

	}
}
