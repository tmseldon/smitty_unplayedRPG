using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Old.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                buildIndex = (int)state["lastSceneBuildIndex"];
            }
            yield return SceneManager.LoadSceneAsync(buildIndex);
            RestoreState(state);
        }

        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        public void Delete(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        public bool CheckSaveFile(string saveFile)
        {
            return File.Exists(GetPathFromSaveFile(saveFile));
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            
            /*
             * Intento de JSON
            
            using(StreamReader reader = new StreamReader(path))
            using(JsonReader jsonReader = new JsonTextReader(reader))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<Dictionary<string, object>>(jsonReader);

            }
            */
            
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
            
        }

        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            
            /*
             * Intento de JSON
            using (StreamWriter writer = new StreamWriter(path))
            using (JsonWriter jsonWriter = new JsonTextWriter(writer))
            {
                jsonWriter.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jsonWriter, state);
            }
            */
            
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
            
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();
                if (state.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}