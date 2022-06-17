using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [Header("Dialogue Node settings")]
        [SerializeField] bool _isPlayerSpeaking = false;
        [SerializeField] string _textDialogue;
        [SerializeField] List<string> _nextDialogues = new List<string>();
        [SerializeField] Rect _nodeRect = new Rect(0, 0, 200, 100);

        [Header("Action triggers")]
        [SerializeField] string _onEnterAction;
        [SerializeField] string _onExitAction;

        public string TextDialogue
        {
            get { return _textDialogue; }
#if UNITY_EDITOR            
            set
            {
                if(_textDialogue != value)
                {
                    //Undo.RecordObject(this, "Update on Dialogue Editor");
                    UndoAndDirty.Mark(this, "Update on Dialogue Editor");
                    _textDialogue = value;

                }
            }
#endif
        }

        public bool IsPlayerSpeaking
        {
            get { return _isPlayerSpeaking; }
#if UNITY_EDITOR   
            set 
            {
                UndoAndDirty.Mark(this, "Change Speaker");
                _isPlayerSpeaking = value; 
            }
#endif
        }

        public List<string> NextDialogues { get { return _nextDialogues; } }
        public Rect NodeRect { get { return _nodeRect; } }
        public string OnEnterAction { get { return _onEnterAction; } }
        public string OnExitAction { get { return _onExitAction; } }

#if UNITY_EDITOR
        public void NodeRectUpdatePos(Vector2 newPosstion)
        {
            //Undo.RecordObject(this, "Update on Node Position");
            UndoAndDirty.Mark(this, "Update on Node Position");
            _nodeRect.position = newPosstion;

        }

        public void AddLink(string idLinkNode)
        {
            //Undo.RecordObject(this, "Added Link with Dialogue Node");
            UndoAndDirty.Mark(this, "Added Link with Dialogue Node");
            _nextDialogues.Add(idLinkNode);

        }

        public void RemoveLink(string idLinkNode)
        {
            //Undo.RecordObject(this, "Removed Link with Dialogue Node");
            UndoAndDirty.Mark(this, "Removed Link with Dialogue Node");
            _nextDialogues.Remove(idLinkNode);

        }
#endif

    }
}
