using UnityEngine;

public class GamePhaseCombat : GamePhase
{
    public GamePhaseCombat(TurnManager turnManager) : base(turnManager) { }

    public override void EnterPhase()
    {
        Debug.Log("--- FAZA WALKI (Automatyczna) ---");
        manager.OnCombatPhaseStart?.Invoke();
    }

    public override void UpdatePhase()
    {
        // To jest przeniesione ze starego GameLogicManager.
        // Pozwala zachować dotychczasową logikę wykrywania końca walki,
        // ale sprawdzanie nie zużywa zasobów poza tą Fazą
        if (manager.PlayerTargets.Count == 0 && manager.EnemieUnitsManagers.Count == 0 
            && ResourceManager.Instance.FindResource(ResourceType.food)?.Amount == 0
            && EnemieResourceManager.Instance.FindResource(ResourceType.food)?.Amount == 0) 
        {
            manager.ChangePhase(new GamePhaseEnd(manager));
        }
    }

    public override void ExitPhase()
    {
        manager.OnCombatPhaseEnd?.Invoke();
    }
}
