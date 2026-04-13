using System;
using System.Collections.Generic;
using UnityEngine;

// Klasa "Matka" zawiadująca turą 
public class TurnManager : MonoBehaviourSingleton<TurnManager>
{
    public const int MAX_CARDS_IN_HAND = 5;
    public const int MAX_ACTION_POINTS = 3;
    public const int ROUND_START_FOOD = 100;

    public Action OnCombatPhaseStart;
    public Action OnCombatPhaseEnd;
    public Action OnRoundEnd;



    public List<EnemieUnitsManager> EnemieUnitsManagers = new();
    public List<UnitsManager> PlayerUnitsManagers = new();

    public List<ITargetable> EnemieTargets = new();
    public List<ITargetable> PlayerTargets = new();

    private GamePhase _currentPhase;
    public GamePhase CurrentPhase => _currentPhase;

    // Używane do uruchomienia gry przyciskiem "FirstTurn"
    public void StartGameManually()
    {
        if (_currentPhase == null)
            ChangePhase(new GamePhaseStart(this));
    }

    private void Update()
    {
        _currentPhase?.UpdatePhase();
    }

    public void ChangePhase(GamePhase newPhase)
    {
        _currentPhase?.ExitPhase();

        _currentPhase = newPhase;
        _currentPhase.EnterPhase();
    }

    public void EndTacticalPhaseRequest()
    {
        if (_currentPhase is GamePhaseTactical)
        {
            ChangePhase(new GamePhaseCombat(this));
        }
    }

    public void OnCardGuaranteedPlayed(CardHandler card)
    {
        _currentPhase?.OnCardPlayed(card);
    }

    public void ClearLists()
    {
        EnemieTargets.Clear();
        PlayerTargets.Clear();
        PlayerUnitsManagers.Clear();
        EnemieUnitsManagers.Clear();
    }
}