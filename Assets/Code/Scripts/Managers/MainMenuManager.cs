using Code.Scripts.Wrappers;

namespace Code.Scripts.Managers
{
	public class MainMenuManager : BaseMonoBehaviour
	{

		public void ClickOnStartNewGameButton()
		{
			StartCoroutine(GameSaveManager.GetInstance().StartNewGame());
		}

	}
}
