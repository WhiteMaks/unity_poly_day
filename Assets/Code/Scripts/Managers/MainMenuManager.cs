using UnityEngine;

namespace Code.Scripts.Managers
{
	public class MainMenuManager : MonoBehaviour
	{

		public void ClickOnStartNewGameButton()
		{
			StartCoroutine(GameSaveManager.GetInstance().StartNewGame());
		}

	}
}
