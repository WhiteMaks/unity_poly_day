using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Managers
{
	public class GameSaveManager : MonoBehaviour
	{
		private static GameSaveManager _instance;

		[SerializeField] private byte worldSceneIndex;

		public static GameSaveManager GetInstance()
		{
			return _instance;
		}

		private void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
				return;
			}

			Destroy(gameObject);
		}

		private void Start()
		{
			DontDestroyOnLoad(gameObject);
		}

		public IEnumerator StartNewGame()
		{
			var loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

			yield return null;
		}

	}
}
