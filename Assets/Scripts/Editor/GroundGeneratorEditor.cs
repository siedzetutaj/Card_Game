using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GroundGenerator))]
public class GroundGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GroundGenerator generator = (GroundGenerator)target;

        GUILayout.Space(10); 

        if (GUILayout.Button("Generate Grid"))
        {
            generator.GenerateGrid();
            EditorUtility.SetDirty(generator);
        }

        if (GUILayout.Button("Clear Grid"))
        {
            generator.ClearGrid();
            EditorUtility.SetDirty(generator);
        }
    }
}
