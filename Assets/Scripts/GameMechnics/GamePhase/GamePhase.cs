public abstract class GamePhase
{
    // Wirtualne metody z domyślną pustą implementacją,
    // Klasa dziedzicząca nadpisuje tylko to co ją interesuje.
    // Jest to imo lepsze niz Interfejs bo nie musimy implementowac pustego updatephase gdy
    // nie jest potrzebne
    
    protected TurnManager manager;
    public GamePhase(TurnManager turnManager)
    {
        this.manager = turnManager;
    }
    
    public virtual void EnterPhase() { }
    public virtual void UpdatePhase() { }
    public virtual void ExitPhase() { }

    // Wirtualna metoda obsługująca zagranie karty w danej fazie
    public virtual void OnCardPlayed(CardHandler card) { }
}