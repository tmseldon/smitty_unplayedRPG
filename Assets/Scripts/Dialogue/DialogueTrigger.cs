using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] string _actionName;
        [SerializeField] UnityEvent _onTriggerStartEvent;

        public void Trigger(string actionToTrigger)
        {
            if(string.Equals(_actionName, actionToTrigger))
            {
                _onTriggerStartEvent.Invoke();
            }
        }
    }
}

