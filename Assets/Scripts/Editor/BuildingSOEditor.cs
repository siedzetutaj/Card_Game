using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(BuildingSO), true)]
//zvibecodowany ale wydaje sie git 
public class BuildingSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BuildingSO buildingSO = (BuildingSO)target;

        GUILayout.Space(10);
        
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Create Next Level Upgrade", GUILayout.Height(30)))
        {
            CreateNextLevel(buildingSO);
        }
        GUI.backgroundColor = Color.white;
    }

    private void CreateNextLevel(BuildingSO currentLevel)
    {
        if (currentLevel.NextLevelPrefab != null)
        {
            Debug.LogWarning("This building already has a Next Level set!");
            return;
        }

        string path = AssetDatabase.GetAssetPath(currentLevel);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning("Please save the current BuildingSO to the project first.");
            return;
        }

        string directory = Path.GetDirectoryName(path);
        string originalName = currentLevel.name;
        
        // Find the base name and level number
        string baseName = originalName;
        int nextLevelNum = 2;

        if (originalName.Contains("_Lvl"))
        {
            int index = originalName.LastIndexOf("_Lvl");
            string numStr = originalName.Substring(index + 4);
            if (int.TryParse(numStr, out int currentNum))
            {
                baseName = originalName.Substring(0, index);
                nextLevelNum = currentNum + 1;
            }
        }

        string newName = $"{baseName}_Lvl{nextLevelNum}";
        string newPath = AssetDatabase.GenerateUniqueAssetPath($"{directory}/{newName}.asset");

        AssetDatabase.CopyAsset(path, newPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        BuildingSO nextLevelSO = AssetDatabase.LoadAssetAtPath<BuildingSO>(newPath);
        
        // Ensure BaseBuildingSO is set correctly
        if (currentLevel.BaseBuildingSO == null)
        {
            currentLevel.BaseBuildingSO = currentLevel;
            EditorUtility.SetDirty(currentLevel);
        }
        
        nextLevelSO.BaseBuildingSO = currentLevel.BaseBuildingSO;
        nextLevelSO.NextLevelPrefab = null; // Clear the copy's next level just in case
        
        currentLevel.NextLevelPrefab = nextLevelSO;
        
        EditorUtility.SetDirty(nextLevelSO);
        EditorUtility.SetDirty(currentLevel);
        
        AssetDatabase.SaveAssets();
        
        Debug.Log($"Created next level upgrade: {newName} at {newPath}");
        
        // Select the new SO to easily edit it
        Selection.activeObject = nextLevelSO;
    }
}
