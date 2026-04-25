using UnityEngine;
using UnityEditor;

public class MissingScriptCleaner : Editor
{
    [MenuItem("Tools/Remove Missing Scripts From Selection")]
    static void RemoveMissingScripts()
    {
        var go = Selection.activeGameObject;
        if (go == null)
        {
            Debug.LogWarning("Zaznacz obiekt w Hierarchy!");
            return;
        }
        
        int count = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
        Debug.Log($"Usunięto {count} brakujących skryptów z {go.name}");
    }
}
