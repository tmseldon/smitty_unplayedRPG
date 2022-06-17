using Newtonsoft.Json.Linq;
using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Control
{
    public class EndGameController : MonoBehaviour, IJsonSaveable
    {
        private bool _isTriggerEndGame = false;
        public bool IsTriggerEndGame { get { return _isTriggerEndGame; } }

        public void ChangeEndGameState()
        {
            _isTriggerEndGame = true;
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(_isTriggerEndGame);
        }

        public void RestoreFromJToken(JToken state)
        {
            _isTriggerEndGame = state.ToObject<bool>();
        }
    }
}


