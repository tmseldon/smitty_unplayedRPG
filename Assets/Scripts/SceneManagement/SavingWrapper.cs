using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float timeStartingLevel = 0.5f;

        JsonSavingSystem savingSystem;
        private const string SAVING_FILE_NAME = "save";

        private void Awake()
        {
            savingSystem = GetComponent<JsonSavingSystem>();
            Restart();
        }

        IEnumerator LoadLastScene()
        {
            
            yield return savingSystem.LoadLastScene(SAVING_FILE_NAME);
            Fader faderSystem = FindObjectOfType<Fader>();
            faderSystem.InmidiatlyFadeOut();
            yield return faderSystem.FadeOutFX(timeStartingLevel);
        }

        //void Update()
        //{
        
        //    if(Input.GetKeyDown(KeyCode.L))
        //    {
        //        Load();
        //    }

        //    if (Input.GetKeyDown(KeyCode.S))
        //    {
        //        Save();
        //    }

        //    //*************For Debug Only

        //    if(Input.GetKeyDown(KeyCode.Delete))
        //    {
        //        Delete();
        //    }

        //    if(Input.GetKeyDown(KeyCode.C))
        //    {
        //        Debug.Log($"Existe savefile?: {savingSystem.CheckSaveFile(SAVING_FILE_NAME)}");
        //    }


        //    //*******************
        //}

        public void Restart()
        {
            StartCoroutine(LoadLastScene());
        }

        public void Save()
        {
            savingSystem.Save(SAVING_FILE_NAME);
        }

        public void Load()
        {
            savingSystem.Load(SAVING_FILE_NAME);
        }

        public void Delete()
        {
            savingSystem.Delete(SAVING_FILE_NAME);
        }
    }


}
