using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] string _playerName;
        [SerializeField] Sprite _avatarPlayer;
        
        private Dialogue _currentDialogue;
        private DialogueNode _currentNode = null;
        private AIConversant _currentConversant = null;
        private bool _isChoosing = false;

        public event Action OnConversationUpdated;
        public bool IsChoosing
        {
            get { return _isChoosing; }
        }

        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
        {
            _currentConversant = newConversant;
            _currentDialogue = newDialogue;
            _currentNode = _currentDialogue.GetRootNode();
            TriggerEnterAction();
            OnConversationUpdated();
        }

        public string GetInfoAvatar(out Sprite avatarPic)
        {
            //Debug.Log($"{_currentNode.name} y el estado del speakerPlayer es {_currentNode.IsPlayerSpeaking}");

            if (_isChoosing)
            {
                avatarPic = _avatarPlayer;
                return _playerName;
            }
            else if(_currentNode.IsPlayerSpeaking)
            {
                avatarPic = _avatarPlayer;
                return _playerName;
            }
            else
            {
                avatarPic = _currentConversant.AvatarAI;
                return _currentConversant.PlayerAI;
            }
        }

        public bool IsActive()
        {
            return _currentDialogue != null;
        }

        public string GetText()
        {
            if(_currentNode == null) { return ""; }

            return _currentNode.TextDialogue;
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return _currentDialogue.GetPlayerChildren(_currentNode);
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            _currentNode = chosenNode;
            TriggerEnterAction();
            _isChoosing = false;

            if(HasNext())
            {
                Next();
            }
            else
            {
                Quit();
            }
            
        }


        public void Next()
        {
            int numberPlayerResponse = _currentDialogue.GetPlayerChildren(_currentNode).Count();
            if(numberPlayerResponse > 0)
            {
                _isChoosing = true;
                TriggerExitAction();
                OnConversationUpdated();
                return;
            }

            DialogueNode[] nextChildren = _currentDialogue.GetAIChildren(_currentNode).ToArray();
            int selectRandom = UnityEngine.Random.Range(0, nextChildren.Length);
            TriggerExitAction();
            _currentNode = nextChildren[selectRandom];
            TriggerEnterAction();
            OnConversationUpdated();            
        }

        public bool HasNext()
        {
            return _currentDialogue.GetAllChildren(_currentNode).Count() > 0;
        }

        public void Quit()
        {
            _currentDialogue = null;
            TriggerExitAction();
            _currentNode = null;
            _isChoosing = false;
            _currentConversant = null;
            OnConversationUpdated();
        }

        private void TriggerEnterAction()
        {
            if(_currentNode != null)
            {
                TriggerAction(_currentNode.OnEnterAction);
            }
        }

        private void TriggerExitAction()
        {
            if (_currentNode != null)
            {
                TriggerAction(_currentNode.OnExitAction);
            }
        }

        private void TriggerAction(string nameAction)
        {
            if(string.IsNullOrEmpty(nameAction)) { return; }

            foreach(DialogueTrigger trigger in _currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(nameAction);
            }
        }
    }
}
