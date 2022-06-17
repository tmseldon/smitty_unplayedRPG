using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


namespace RPG.Core
{
    public class PersistentSpawnObjects : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;
        [SerializeField] string _mainMenuName = "MainMenu";

        static bool hasSpawned = false;
        int currentSceneIndex;

        private void Awake()
        {
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentSceneIndex == SceneManager.GetSceneByName(_mainMenuName).buildIndex)
            {
                RemoveSpawnedPersistantObjects();
                hasSpawned = false;
                return;
            }

            if (hasSpawned) return;

            SpawnPersistantObjects();
            hasSpawned = true;
        }

        private void SpawnPersistantObjects()
        {
            if(persistentObjectPrefab)
            {
                GameObject persistantObject = Instantiate(persistentObjectPrefab);
                DontDestroyOnLoad(persistantObject);
            }
        }

        private void RemoveSpawnedPersistantObjects()
        {
            GameObject persistantObjects = GameObject.FindGameObjectWithTag("Persistant");
            if (persistantObjects)
            {
                Destroy(persistantObjects);
            }
        }
    }

}

