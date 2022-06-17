using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue_", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] 
        List<DialogueNode> _dialogues = new List<DialogueNode>();

        Vector2 _offsetNewNode = new Vector2(200f, 0);        
        Dictionary<string, DialogueNode> _nodesLookUp = new Dictionary<string, DialogueNode>();

        //SE incluye Onvalidate, porque en el build del juego no se llama onvalidate ya que pertenece a los cambios en editor


        private void Awake()
        {

            //#if UNITY_EDITOR
            //            if (_dialogues.count == 0)
            //            {
            //                createnewnode(null);
            //            }
            //#endif

            OnValidate();
        }


        private void OnValidate()
        {
            _nodesLookUp.Clear();
            foreach(DialogueNode nodes in GetAllNodes())
            {
                _nodesLookUp.Add(nodes.name, nodes);
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return _dialogues;
        }

        public DialogueNode GetRootNode()
        {
            return _dialogues[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach(string childNodes in parentNode.NextDialogues)
            {
                if(_nodesLookUp.ContainsKey(childNodes))
                {
                    yield return _nodesLookUp[childNodes];
                }
            }
        }

        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode parentNode)
        {
            foreach (DialogueNode childNode in GetAllChildren(parentNode))
            {
                if (childNode.IsPlayerSpeaking)
                {
                    yield return childNode;
                }
            }
        }    

        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode parentNode)
        {
            foreach(DialogueNode childNode in GetAllChildren(parentNode))
            {
                if(!childNode.IsPlayerSpeaking)
                {
                    yield return childNode;
                }
            }
        }


#if UNITY_EDITOR
        public void CreateNewNode(DialogueNode parent)
        {
            DialogueNode newChildNode = MakeNode(parent);
            //Undo.RegisterCompleteObjectUndo(newChildNode, "New Dialogue Node Added");
            UndoAndDirty.Mark(newChildNode, "New Dialogue Node Added");

            //REmove bug when creating a new Dialogue, this happen because when creates the bew Node,
            //it does not have a current path to record the UNDO, making all the system freeze
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                //Undo.RecordObject(this, "Added New Dialogue Node");
                UndoAndDirty.Mark(this, "Added New Dialogue Node");
            }
            //Add to list and Dictionary of nodes
            UpdateChildNode(newChildNode);
        }

        private DialogueNode MakeNode(DialogueNode parent)
        {

            //Creating new childNode with UniqueID
            DialogueNode newChildNode = CreateInstance<DialogueNode>();
            newChildNode.name = Guid.NewGuid().ToString();

            if (parent != null)
            {
                //Updateing parent registry
                parent.AddLink(newChildNode.name);
                newChildNode.NodeRectUpdatePos(parent.NodeRect.position + _offsetNewNode);
                
                //Caso contrario por defecto el nodo tiene el bool = false
                if(!parent.IsPlayerSpeaking)
                {
                    newChildNode.IsPlayerSpeaking = true;
                }
            }

            return newChildNode;
        }

        private void UpdateChildNode(DialogueNode newChildNode)
        {
            _dialogues.Add(newChildNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode eraseNode)
        {
            //Undo.RecordObject(this, "Deleted Dialogue Node");
            UndoAndDirty.Mark(this, "Deleted Dialogue Node");
            _dialogues.Remove(eraseNode);
            OnValidate();
            CleanDanglingChildren(eraseNode);
            Undo.DestroyObjectImmediate(eraseNode);
        }

        private void CleanDanglingChildren(DialogueNode eraseNode)
        {
            foreach (DialogueNode dialogue in GetAllNodes())
            {
                dialogue.RemoveLink(eraseNode.name);
            }
        }

        public bool IsChildFrom(DialogueNode parent, DialogueNode potentialChild)
        {
            string result = parent.NextDialogues.Find(x => x.Equals(potentialChild.name));
            if(result !=null)
            {
                return true;
            }

            return false;
        }
#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR

            if (_dialogues.Count == 0)
            {
                DialogueNode rootNode = MakeNode(null);
                UpdateChildNode(rootNode);
            }

            if (! String.IsNullOrEmpty(AssetDatabase.GetAssetPath(this)))
            {
                foreach(DialogueNode node in GetAllNodes())
                {
                    if (String.IsNullOrEmpty(AssetDatabase.GetAssetPath(node)))
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
        }
    }

}
