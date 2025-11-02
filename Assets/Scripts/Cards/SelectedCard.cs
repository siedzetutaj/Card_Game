using UnityEngine;

public class SelectedCard : MonoBehaviourSingleton<SelectedCard>
{
    private CardHandler _card;
    public CardHandler Card
    {
        get => _card;
        set
        {
            if (_card != null)
                _card.DisableHighlight();
            _card = value;
        }
    }
}
