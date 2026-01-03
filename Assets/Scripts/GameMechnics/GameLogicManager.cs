using System;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicManager : MonoBehaviourSingleton<GameLogicManager>
{
    public Action OnEndTurn;
    public Action OnEndFight;
    public Action OnAllUnitsDead;

    public List<Transform> SpawnPoints;

    public List<EnemieUnitsManager> EnemieUnitsManagers = new();
    public List<UnitsManager> PlayerUnitsManagers = new();

    public List<UnitHandler> EnemieUnits = new();
    public List<UnitHandler> PlayerUnits = new();

    public List<ITargetable> PlayerBuildingsToTarget = new();
    public List<ITargetable> EnemieBuildingsToTarget = new();

    public bool IsFight = false;

    public CombatPhase CurrentPhase { get; private set; } = CombatPhase.None;

    [SerializeField] private int _turn = 0;
    private int _moneyAmount = 3;

    private DeckManager _deckManager => DeckManager.Instance;
    private EnemieAttackProjection _enemieAttackProjection => EnemieAttackProjection.Instance;
    //temp solution
    private void Update()
    {
        CheckCombatPhase();

        if (IsFight && PlayerUnitsManagers.Count == 0 && EnemieUnitsManagers[0].Units.Count == 0) 
        {
            OnDeathOfAllUnits();
        }
    }

    #region Units
    public void SpawnEnemieUnits()
    {
        foreach(var enemieUnitsManager in EnemieUnitsManagers)
        {
            enemieUnitsManager.Initialize(enemieUnitsManager.UnitData,
                SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Count - 1)].position, false);
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
    public void OnFightEnd(bool hasPlayerLost)
    {
        if (!IsFight)
        {
            IsFight = true;
            //DestroyAllUnits();
            //ClearLists();
            OnEndFight?.Invoke();
            //NextTurnSetup();
        }
    }
    public void OnDeathOfAllUnits()
    {
        IsFight = false;
        OnAllUnitsDead?.Invoke();
        //DestroyAllUnits();
        ClearLists();
        OnEndFight?.Invoke();
        NextTurnSetup();
    }
    public void EndTurnCheck()
    {
        if(_deckManager.IsHandEmpty())
            EndTurn();
    }
    public void EndTurn()
    {
        OnEndTurn?.Invoke();
        _deckManager.DiscardAllCardsInHand();
        IsFight = false;
        CurrentPhase = CombatPhase.Units;

        Debug.Log("End Turn");
        SpawnEnemieUnits();
        UnifyUnits();
    }
    public void FirstTurn()
    {
        if (_turn == 0)
        {
            _turn++;
            _deckManager.OnFirstTurn(_moneyAmount);
            TempEnemieScaling();
            EnemieAttackProjectionSetup();
        }
        else
            NextTurnSetup();

    }
    private void NextTurnSetup()
    {
        _turn++;
        TempEnemieScaling();
        _deckManager.NextTurn(_moneyAmount);
        EnemieAttackProjectionSetup();
    }
    private void TempEnemieScaling()
    {
        foreach (var enemieUnitsManager in EnemieUnitsManagers)
        {
            float scaleFactor = 1.2f;
            enemieUnitsManager.UnitData.UnitAmount =
                 Mathf.RoundToInt
                 (enemieUnitsManager.UnitData.UnitAmount * scaleFactor);
        }
    }
    private void EnemieAttackProjectionSetup()
    {
        _enemieAttackProjection.ClearAttackProjections();
        foreach (var enemieUnitsManager in EnemieUnitsManagers)
        {
            _enemieAttackProjection.SetAttackProjection(enemieUnitsManager.UnitData.UnitSprite,
                enemieUnitsManager.UnitData.UnitAmount);
        }
    }
    public void EnemieDefeated()
    {
        Debug.Log("Enemie Defeated");
    }
    public void CheckCombatPhase()
    {
        EnemieUnits.RemoveAll(x => x == null);
        PlayerUnits.RemoveAll(x => x == null);
        PlayerBuildingsToTarget.RemoveAll(x => x == null);

        if (CurrentPhase == CombatPhase.Units &&
            (PlayerUnits.Count == 0 || EnemieUnits.Count == 0))
        {
            CurrentPhase = CombatPhase.Buildings;
        }
    }
    #endregion
    #region Utilities
    private void ClearLists()
    {
        EnemieUnits.Clear();
        PlayerUnits.Clear();
        PlayerUnitsManagers.Clear();
    }
    #endregion
}
public enum CombatPhase
{
    Units,
    Buildings,
    None
}