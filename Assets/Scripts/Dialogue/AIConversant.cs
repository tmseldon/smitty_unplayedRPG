using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] string _playerAI;
        [SerializeField] Sprite _avatarAI;
        [SerializeField] Dialogue dialogueAI;
        [Header("EndGame params")]
        [SerializeField] bool _isLastDialogue = false;
        [SerializeField] Dialogue _endGameDailogue = null;

        public string PlayerAI { get { return _playerAI; } }
        public Sprite AvatarAI { get { return _avatarAI; } }

        public CursorType GetCursor()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRayCast(PlayerController callingController)
        {
            if(dialogueAI == null) { return false; }
            
            if (Input.GetMouseButton(0))
            {
                if(_isLastDialogue && DetermineIsEndGame(callingController))
                {
                    callingController.GetComponent<PlayerConversant>().StartDialogue(this, _endGameDailogue);
                }
                else
                {
                    callingController.GetComponent<PlayerConversant>().StartDialogue(this, dialogueAI);
                }
            }

            return true;
        }

        private bool DetermineIsEndGame(PlayerController controller)
        {
            return controller.GetComponent<EndGameController>().IsTriggerEndGame;
        }
    }

}
