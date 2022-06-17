using RPG.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    [RequireComponent(typeof(Button))]
    public class BackMainMenu : MonoBehaviour
    {
        Button _buttonBack;
        MainMenu _mainMenu;

        private void Awake()
        {
            _mainMenu = FindObjectOfType<MainMenu>();
            _buttonBack = GetComponent<Button>();
        }

        private void Start()
        {
            _buttonBack.onClick.AddListener(_mainMenu.BackToMainMenu);
        }
    }

}
