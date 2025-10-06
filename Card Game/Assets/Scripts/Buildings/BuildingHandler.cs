using UnityEngine;
using UnityEngine.UI;

public class BuildingHandler : InteractableObject
{
    [SerializeField] private Image _image;
    
    private BuildingSO _buildingSO;
    private SelectedCard _selectedCard;

    private UnitData _unitData;
    public UnitData UnitData
    {
        get => _unitData;
        private set => _unitData = value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _selectedCard = SelectedCard.Instance; 
    }

    public void Initialize(BuildingSO buildingSO)
    {
        _buildingSO = buildingSO;
        _image.sprite = buildingSO.Sprite;
        _unitData = new(buildingSO.UnitSO.UnitData);
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
