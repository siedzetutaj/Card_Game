using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingHandler : InteractableObject
{
    [SerializeField] private Image _image;
    
    private BuildingSO _buildingSO;
    private SelectedCard _selectedCard;

    private List<BuildingOnEndTurnEffectSO> _onEndTurnEffects = new();

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
        GameLogicManager.Instance.OnEndTurn += ApplyOnEndturnEffects;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        GameLogicManager.Instance.OnEndTurn -= ApplyOnEndturnEffects;
    }

    public void Initialize(BuildingSO buildingSO)
    {
        _buildingSO = buildingSO;
        _image.sprite = buildingSO.Sprite;
        _unitData = new (buildingSO.UnitSO.UnitData);
        _onEndTurnEffects = new (buildingSO.OnEndTurnEffects);
    }
    protected override void OnObjectClicked()
    {

        bool canCardBePlayed = (_selectedCard.Card != null &&
            _selectedCard.Card.CardType == CardType.Recruitment);

        if (canCardBePlayed)
        {
            CardHandler card = SelectedCard.Instance.Card;
            card.OnCardPlayed(gameObject);
        }
    }
    protected virtual void ApplyOnEndturnEffects()
    {
        foreach (var effect in _onEndTurnEffects)
        {
            effect.ApplyOnEndTurnEffect(this);
        }
    }
}
