using UnityEngine;

public class GamePhaseStart : GamePhase
{
    public GamePhaseStart(TurnManager turnManager) : base(turnManager) { }

    public override void EnterPhase()
    {
        Debug.Log("--- FAZA STARTU ---");

        // 1. Odnowienie Punktów Akcji (do 3)
        ResourceManager.Instance.FindResource(ResourceType.money)?.SetAmount(TurnManager.MAX_ACTION_POINTS);
        
        

        // 3. Dobieranie kart (dobieramy DO 5 w ręce, a nie wyrzucamy resztę w ciemno)
        int cardsInHand = DeckManager.Instance.GetHandCount();
        int cardsToDraw = TurnManager.MAX_CARDS_IN_HAND - cardsInHand;
        
        if (cardsToDraw > 0)
        {
            DeckManager.Instance.DrawCards(cardsToDraw);
        }
        
        manager.ChangePhase(new GamePhaseTactical(manager));
    }
}
