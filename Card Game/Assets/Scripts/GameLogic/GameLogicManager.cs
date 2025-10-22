using System.Collections.Generic;
using UnityEngine;

public class GameLogicManager : MonoBehaviourSingleton<GameLogicManager>
{
    public List<Transform> SpawnPoints;

    public List<UnitsManager> EnemieUnitsManagers = new();
    public List<UnitsManager> PlayerUnitsManagers = new();
    public List<UnitHandler> EnemieUnits = new();
    public List<UnitHandler> PlayerUnits = new();
    
    public UnitSO UnitSO;
    public bool hasFightEnded = false;
    public void EndTurn()
    {
        hasFightEnded = false;
        Debug.Log("End Turn");
        SpawnEnemies();
        UnifyUnits();
    }

    private void UnifyUnits()
    {
        foreach (var enemieUnitsManager in EnemieUnitsManagers)
        {
            EnemieUnits.AddRange(enemieUnitsManager.Units);
        }
        foreach (var playerUnitsManager in PlayerUnitsManagers)
        {
            PlayerUnits.AddRange(playerUnitsManager.Units);
        }
    }
    public void SpawnEnemies()
    {
        foreach(var enemieUnitsManager in EnemieUnitsManagers)
        {
            enemieUnitsManager.Initialize(UnitSO.UnitData,
                SpawnPoints[Random.Range(0, SpawnPoints.Count - 1)], false);
        }
    }
    public void OnFightEnd()
    {
        if (!hasFightEnded)
        {
            hasFightEnded = true;
            DestroyAllUnits();
            ClearLists();
            DeckManager.Instance.NextTurn();
        }
    }
    private void ClearLists()
    {
        EnemieUnits.Clear();
        PlayerUnits.Clear();
        PlayerUnitsManagers.Clear();
    }
    private void DestroyAllUnits()
    {
        PlayerUnits.RemoveAll(x => x == null);
        
        foreach(UnitHandler playerUnits in PlayerUnits)
        {
            playerUnits.DestroyUnit();
        }
        
        EnemieUnits.RemoveAll(x => x == null);
        
        foreach (UnitHandler enemieUnits in EnemieUnits)
        {
            //TODO: Deal damage to player base
            if (enemieUnits == null) continue;

            enemieUnits.DestroyUnit();
        }
    }
}
