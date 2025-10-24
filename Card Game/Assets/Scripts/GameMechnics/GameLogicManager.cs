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

    private DeckManager _deckManager => DeckManager.Instance;
    #region Units
    public void SpawnEnemieUnits()
    {
        foreach(var enemieUnitsManager in EnemieUnitsManagers)
        {
            enemieUnitsManager.Initialize(UnitSO.UnitData,
                SpawnPoints[Random.Range(0, SpawnPoints.Count - 1)], false);
        }
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
    #endregion
    #region TurnManagement
    public void OnFightEnd(bool isPlayerUnit)
    {
        if (!hasFightEnded)
        {
            hasFightEnded = true;
            DestroyAllUnits();
            ClearLists();
            SetHealth(isPlayerUnit);
            DeckManager.Instance.NextTurn();
        }
    }
    private void SetHealth(bool isPlayerUnit)
    {
        if(isPlayerUnit)
            ResourceManager.Instance.FindResource(ResourceType.population).Amount--;
        else
            EnemieManager.Instance.HealthPoints--;
    }
    public void EndTurnCheck()
    {
        if(_deckManager.IsHandEmpty())
            EndTurn();
    }
    public void EndTurn()
    {
        _deckManager.DiscardAllCardsInHand();
        hasFightEnded = false;
        Debug.Log("End Turn");
        SpawnEnemieUnits();
        UnifyUnits();
    }
    #endregion
    public void EnemieDefeated()
    {
        Debug.Log("Enemie Defeated");
    }
    #region Utilities
    private void ClearLists()
    {
        EnemieUnits.Clear();
        PlayerUnits.Clear();
        PlayerUnitsManagers.Clear();
    }
    #endregion
}
