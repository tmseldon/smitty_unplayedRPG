using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class SceneManagerMain : MonoBehaviour
    {
        [SerializeField] string _firstLevelName;

        private const string MAIN_MENU = "MainMenu";

        public void GoToMain() => SceneManager.LoadScene(MAIN_MENU);
        public void GoToFirstLevel() => SceneManager.LoadScene(_firstLevelName);
        public void QuitGame() => Application.Quit();

        public bool IsThisMainMenu()
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            return currentScene == SceneManager.GetSceneByName(MAIN_MENU).buildIndex;
        }
    }
}

