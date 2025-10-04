using UnityEngine;
using UnityEngine.UI;

public class GroundTileHandler : InteractableObject
{
    private bool _isOccupied = false;
    private SelectedCard _selectedCard;

    protected override void OnEnable()
    {
        base.OnEnable();
        _selectedCard = SelectedCard.Instance;
    }

    protected override void OnObjectClicked()
    {
        bool canCardBeplaced = (_selectedCard.Card != null &&
            _selectedCard.Card.CardType == CardType.Building && !_isOccupied);
        
        if (canCardBeplaced)
        {
            CardHandler card = SelectedCard.Instance.Card;
            card.OnCardPlayed(gameObject);
        }
    }
    protected override void OnObjectMouseEnter()
    {
    }
    protected override void OnObjectMouseExit()
    {
    }
}
