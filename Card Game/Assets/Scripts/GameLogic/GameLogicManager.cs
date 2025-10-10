using System.Collections.Generic;
using UnityEngine;

public class GameLogicManager : MonoBehaviourSingleton<GameLogicManager>
{
    public List<Transform> SpawnPoints;
    public UnitsManager EnemieUnits;
    public UnitsManager PlayerUnits;
    public UnitSO UnitSO;
    public void EndTurn()
    { 
        Debug.Log("End Turn");
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        EnemieUnits.Initialize(UnitSO.UnitData, SpawnPoints[0], false);
    }
}
