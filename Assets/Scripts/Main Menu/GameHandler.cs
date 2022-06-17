using RPG.FX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.SceneManagement
{
    public class GameHandler : MonoBehaviour
    {
        [SerializeField] Canvas _menuScreen;
        [SerializeField] Canvas _menuGameOver;
        [SerializeField] AudioClip _pauseFX;
        [SerializeField] [Range(0f, 1f)] float _volume = 0.5f;

        private MusicManager _musicManager;
        private SceneManagerMain _sceneManager;
        private SavingWrapper _savingWrapper;

        private bool _isPause = false;
        private bool _isGameOver = false;

        //Agregar condición de si está muerto player, no debe activarse el pause

        public bool IsPause { get { return _isPause; } }

        private void Awake()
        {
            _sceneManager = FindObjectOfType<SceneManagerMain>();
            _savingWrapper = FindObjectOfType<SavingWrapper>();
        }

        void Start()
        {
            _menuScreen.enabled = false;
            _menuGameOver.enabled = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !_isPause && !_isGameOver)
            {
                GetPauseMenu();
            }
        }

        //MAIN MENU OPTIONS

        public void GetPauseMenu()
        {
            _isPause = true;
            AudioSource.PlayClipAtPoint(_pauseFX, Camera.main.transform.position, _volume);
            _menuScreen.enabled = true;
            ToogleSystems(false);
            CheckSourceAudio();
            _musicManager.PauseOST();  
        }

        public void ResumeGame()
        {
            _menuScreen.enabled = false;
            ToogleSystems(true);
            CheckSourceAudio();
            _musicManager.PlayOST();
            _isPause = false;
        }

        public void BackToMainMenu()
        {
            ToogleSystems(true);
            _sceneManager.GoToMain();
        }

        public void SaveGame()
        {
            _savingWrapper.Save();
            ResumeGame();
        }

        public void QuitGame()
        {
            _sceneManager.QuitGame();
        }


        //HANDLING GAME OPTIONS

        public void StartNewGame()
        {
            _savingWrapper.Delete();
            _savingWrapper.Restart();
        }

        public void GameOver()
        {
            _isGameOver = true;
            _menuGameOver.enabled = true;
        }

        public void RestartGame()
        {
            _isGameOver = false;
            _savingWrapper.Restart();
        }


        private void ToogleSystems(bool status)
        {
            if (status)
            {
                Time.timeScale = 1;
                //Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;
            }
            else
            {
                Time.timeScale = 0;
                //Cursor.lockState = CursorLockMode.None;
                //Cursor.visible = true;
            }
        }

        private void CheckSourceAudio()
        {
            _musicManager = FindObjectOfType<MusicManager>();
        }
    }
}