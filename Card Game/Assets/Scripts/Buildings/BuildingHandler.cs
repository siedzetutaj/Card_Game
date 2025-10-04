using UnityEngine;
using UnityEngine.UI;

public class BuildingHandler : InteractableObject
{
    [SerializeField] private Image _image;
    
    private BuildingSO _buildingSO;
    private SelectedCard _selectedCard;

    public void Initialize(BuildingSO buildingSO)
    {
        _buildingSO = buildingSO;
        _image.sprite = buildingSO.Sprite;
    }
    protected override void OnObjectClicked()
    {

        bool canCardBeplaced = (_selectedCard.Card != null &&
            _selectedCard.Card.CardType == CardType.Recruitment);

        if (canCardBeplaced)
        {
            CardHandler card = SelectedCard.Instance.Card;
            card.OnCardPlayed(gameObject);
        }
    }
}
