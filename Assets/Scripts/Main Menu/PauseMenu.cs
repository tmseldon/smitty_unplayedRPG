using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.SceneManagement
{
    public class PauseMenu : MonoBehaviour
    {
        Button _pauseMenu;

        private void Awake()
        {
            _pauseMenu = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _pauseMenu.onClick.AddListener(FindObjectOfType<GameHandler>().GetPauseMenu);
        }
    }


}
