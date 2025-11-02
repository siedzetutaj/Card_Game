using UnityEngine;
using System.Collections.Generic;
public class UnitsSpawnPoints : MonoBehaviourSingleton<UnitsSpawnPoints>
{
    /// <summary>
    /// 0 - Range Units
    /// 1 - Melee Units
    /// </summary>
    [SerializeField] private List<SpawnPointsInRow> SpawnPoints = new();
    
    public Transform FindSpawnPoint(int index)
    {
        foreach(SpawnData spawnData in SpawnPoints[index].Row)
        {
    //TODO: Need to add else 
            if (!spawnData.IsOccupied)
                return spawnData.SpawnPoint;
        }
        return null;
    }
}

[System.Serializable]
public class SpawnPointsInRow
{
    public List<SpawnData> Row = new();
}
[System.Serializable]
public class SpawnData
{
    public Transform SpawnPoint;
    public bool IsOccupied = false;
}