using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Control;
using RPG.Saving;
using Newtonsoft.Json.Linq;

namespace RPG.Cinematics
{
    public class TriggerCinematic : MonoBehaviour, IJsonSaveable
    {
        bool didISeeMovie = false;

        private void OnTriggerEnter(Collider other)
        {
            if(IsPlayerWhoEnter(other.gameObject) && !didISeeMovie)
            {
                GetComponent<PlayableDirector>().Play();
                didISeeMovie = true;
            }
        }

        private bool IsPlayerWhoEnter(GameObject gameObject)
        {
            if(gameObject.tag == "Player") { return true; }

            return false;
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(didISeeMovie);
        }

        public void RestoreFromJToken(JToken state)
        {
            didISeeMovie = state.ToObject<bool>();
        }

        /*
         * ISaveable Implementation
         * 
        public object CaptureState()
        {
            return didISeeMovie;
        }

        public void RestoreState(object state)
        {
            didISeeMovie = (bool)state;
        }
        */
    }

}

