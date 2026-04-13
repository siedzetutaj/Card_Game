using UnityEngine;

public class GamePhaseTactical : GamePhase
{
    private int _cardsPlayedThisTurn = 0;

    public GamePhaseTactical(TurnManager turnManager) : base(turnManager) { }

    public override void EnterPhase()
    {
        Debug.Log("--- FAZA TAKTYCZNA (Zagrania) ---");
        _cardsPlayedThisTurn = 0;
        // W tej fazie czekamy na akcje gracza (zagrywanie kart lub klikniecie przycisku końca)
    }

    public override void OnCardPlayed(CardHandler card)
    {
        _cardsPlayedThisTurn++;
        Debug.Log($"Zagrano kartę. Zagrane karty w tej turze: {_cardsPlayedThisTurn} / {TurnManager.MAX_ACTION_POINTS}");

        if (_cardsPlayedThisTurn >= TurnManager.MAX_ACTION_POINTS)
        {
            // Limit zagranych kart wyczerpany - wymuszamy koniec fazy taktycznej
            manager.ChangePhase(new GamePhaseCombat(manager));
        }
    }

    public override void ExitPhase()
    {
        // Stary GameLogicManager.EndTurn() robił to przed walką - wyrzucamy pozostałe karty z ręki
        DeckManager.Instance.DiscardAllCardsInHand();
        Debug.Log("Koniec Fazy Taktycznej.");
    }
}
