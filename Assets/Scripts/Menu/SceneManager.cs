using UnityEngine;

namespace DefaultNamespace.Menu
{
	[CreateAssetMenu(fileName = "SceneManager", menuName = "Scene Manager", order = 0)]
	public class SceneManager : ScriptableObject
	{
		[SerializeField] private string mainMenuScene;
		[SerializeField] private string gameplaySceneName;
		public void GoToMainMenuScene()
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuScene);

		}
		public void GoToGameplayScene()
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(gameplaySceneName);
		}
	}
}