using UnityEngine;

public class GamePhaseEnd : GamePhase
{
    public GamePhaseEnd(TurnManager turnManager) : base(turnManager) { }

    public override void EnterPhase()
    {
        Debug.Log("--- ZAKOŃCZENIE RUNDY ---");

        manager.OnRoundEnd?.Invoke();
        
        manager.ClearLists();

        // Bezpośrednio puszczamy kolejną rundę
        manager.ChangePhase(new GamePhaseStart(manager));
    }
}
