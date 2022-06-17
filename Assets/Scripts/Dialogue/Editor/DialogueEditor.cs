using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue _selectedDialogue = null;
        Vector2 _scrollPosition;
        const float FRAME_SIZE = 4000f;
        const float BACKGROUND_SIZE = 50f;

        [NonSerialized]
        GUIStyle _nodeStyle;
        [NonSerialized]
        GUIStyle _playerNodeStyle;
        [NonSerialized]
        Vector2 offsetMouse;
        [NonSerialized]
        DialogueNode _draggedNode = null;
        [NonSerialized]
        DialogueNode _addNewNodeTo = null;
        [NonSerialized]
        DialogueNode _deleteNodeFrom = null;
        [NonSerialized]
        DialogueNode _linkingParentNode = null;
        [NonSerialized]
        bool _dragginCanvas = false;
        [NonSerialized]
        Vector2 _draggingCanvasOffset;


        [MenuItem("Window/Dialogue Window")]
        public static void ShowWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAssetAttribute(1)]
        public static bool OnDoubleClickAsset(int instanceID, int line)
        {
            var assetSelected = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
          
            if(assetSelected == null) { return false; }
            ShowWindow();
            return true;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChange;

            _nodeStyle = new GUIStyle();
            _nodeStyle.normal.background = EditorGUIUtility.Load("node2") as Texture2D;
            _nodeStyle.padding = new RectOffset(15, 15, 15, 10);
            _nodeStyle.border = new RectOffset(25, 25, 10, 10);

            _playerNodeStyle = new GUIStyle();
            _playerNodeStyle.normal.background = EditorGUIUtility.Load("node5") as Texture2D;
            _playerNodeStyle.padding = new RectOffset(15, 15, 15, 10);
            _playerNodeStyle.border = new RectOffset(25, 25, 10, 10);
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChange;
        }

        private void OnSelectionChange()
        {
            Dialogue potentialDialogue = Selection.activeObject as Dialogue;
            if (potentialDialogue != null) 
            { 
                _selectedDialogue = potentialDialogue;
                Repaint();
            }
            
        }

        private void OnGUI()
        {
            if (_selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No dialogue selected");
            }
            else
            {
                EditorGUILayout.LabelField(_selectedDialogue.name + (EditorUtility.IsDirty(_selectedDialogue) ? " **" : ""));

                ProcessEvents();

                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

                Rect canvas = GUILayoutUtility.GetRect(FRAME_SIZE, FRAME_SIZE);
                float ratio = FRAME_SIZE / BACKGROUND_SIZE;
                Rect imageSize = new Rect(0, 0, ratio, ratio);
                Texture2D backgroundTexture = Resources.Load("background") as Texture2D;

                GUI.DrawTextureWithTexCoords(canvas, backgroundTexture, imageSize);

                foreach (DialogueNode dialogue in _selectedDialogue.GetAllNodes())
                {
                    DrawConnections(dialogue);
                    DrawNode(dialogue);
                }
                /*
                foreach (DialogueNode dialogue in _selectedDialogue.GetAllNodes())
                {
                    DrawNode(dialogue);
                }
                */

                EditorGUILayout.EndScrollView();

                if (_addNewNodeTo != null)
                {
                    _selectedDialogue.CreateNewNode(_addNewNodeTo);
                    _addNewNodeTo = null;
                }
                if (_deleteNodeFrom != null)
                {
                    _selectedDialogue.DeleteNode(_deleteNodeFrom);
                    _deleteNodeFrom = null;
                }
            }
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && _draggedNode == null)
            {
                _draggedNode = GetClickedNode(Event.current.mousePosition + _scrollPosition);

                if (_draggedNode != null)
                {
                    offsetMouse = Event.current.mousePosition - _draggedNode.NodeRect.position;
                    Selection.activeObject = _draggedNode;
                }
                else
                {
                    _draggingCanvasOffset = Event.current.mousePosition + _scrollPosition;
                    _dragginCanvas = true;
                    Selection.activeObject = _selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && _draggedNode != null)
            {
                _draggedNode.NodeRectUpdatePos(Event.current.mousePosition - offsetMouse);
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && _dragginCanvas)
            {
                _scrollPosition = _draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && _draggedNode != null)
            {
                _draggedNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && _dragginCanvas)
            {
                _dragginCanvas = false;
            }
        }

        private void DrawNode(DialogueNode dialogue)
        {
            GUIStyle style = _nodeStyle;

            if(dialogue.IsPlayerSpeaking)
            {
                style = _playerNodeStyle;
            }
            
            GUILayout.BeginArea(dialogue.NodeRect, style);

            //EditorGUI.BeginChangeCheck();

            dialogue.TextDialogue = EditorGUILayout.TextField(dialogue.TextDialogue);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("+"))
            {
                _addNewNodeTo = dialogue;
            }

            DrawLinkButton(dialogue);

            if (GUILayout.Button("-"))
            {
                _deleteNodeFrom = dialogue;
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawLinkButton(DialogueNode dialogue)
        {
            if (_linkingParentNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    _linkingParentNode = dialogue;
                }
            }
            else if (_linkingParentNode == dialogue)
            {
                if (GUILayout.Button("Cancel"))
                {
                    //Case Cancel Link
                    _linkingParentNode = null;
                }
            }
            else if (_linkingParentNode.NextDialogues.Contains(dialogue.name))
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Unlink"))
                {
                    //Case Unlink Child
                    _linkingParentNode.RemoveLink(dialogue.name);
                    _linkingParentNode = null;
                }
                GUI.color = Color.white;
            }
            else
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Child"))
                {
                    //Case linking new node
                    _linkingParentNode.AddLink(dialogue.name);
                    _linkingParentNode = null;
                }
                GUI.color = Color.white;
            }
        }

        private void DrawConnections(DialogueNode dialogue)
        {
            foreach (DialogueNode childNode in _selectedDialogue.GetAllChildren(dialogue))
            {
                Vector3 startPoint = dialogue.NodeRect.center + new Vector2(dialogue.NodeRect.width / 2, 0);
                Vector3 endingPoint = childNode.NodeRect.center - new Vector2(childNode.NodeRect.width / 2, 0);

                Vector3 offsetBezier = endingPoint - startPoint;
                offsetBezier.y = 0;
                offsetBezier.x *= 0.8f;

                Handles.DrawBezier(startPoint, endingPoint,
                    startPoint + offsetBezier, endingPoint - offsetBezier,
                    Color.white, Texture2D.whiteTexture, 2f);
            }
        }

        private DialogueNode GetClickedNode(Vector2 point)
        {
            DialogueNode foundNode = null;

            foreach(DialogueNode dialogue in _selectedDialogue.GetAllNodes())
            {
                if(dialogue.NodeRect.Contains(point))
                {
                    foundNode = dialogue;
                }
            }

            return foundNode;
        }
    }

}
