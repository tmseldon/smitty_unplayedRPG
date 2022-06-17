using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
namespace RPG.Dialogue
{
    public class UndoAndDirty
    {
        public static void Mark(UnityEngine.Object target, string undoDescription)
        {
            Undo.RecordObject(target, undoDescription);
            EditorUtility.SetDirty(target);
        }
    }
}
#endif
