using UnityEngine;
using UnityEngine.SceneManagement;

namespace Castillo.UI
{
    public class UI_MainMenu : MonoBehaviour
    {
        [SerializeField] private string _gameplaySceneName = "GameScene";

        public void PlayGame()
        {
            SceneManager.LoadScene(_gameplaySceneName);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}