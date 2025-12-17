using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingHandler : InteractableObject
{
    [SerializeField] private Image _image;

    protected BuildingSO _buildingSO;
    private SelectedCard _selectedCard;

    private List<BuildingOnEndTurnEffectSO> _onEndTurnEffects = new();
    private List<BuildingOnEndFightEffectSO> _onEndFightEffects = new();


    protected override void OnEnable()
    {
        base.OnEnable();

        _selectedCard = SelectedCard.Instance; 
        GameLogicManager.Instance.OnEndTurn += ApplyOnEndTurnEffects;
        GameLogicManager.Instance.OnEndFight += ApplyOnEndFightEffects;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        GameLogicManager.Instance.OnEndTurn -= ApplyOnEndTurnEffects;
        GameLogicManager.Instance.OnEndFight -= ApplyOnEndFightEffects;

    }

    public virtual void Initialize(BuildingSO buildingSO)
    {
        _buildingSO = buildingSO;
        _image.sprite = buildingSO.Sprite;
        _onEndTurnEffects = new (buildingSO.OnEndTurnEffects);
        _onEndFightEffects = new (buildingSO.OnEndFightEffects);
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
    protected virtual void ApplyOnEndTurnEffects()
    {
        foreach (var effect in _onEndTurnEffects)
        {
            effect.ApplyOnEndTurnEffect(this);
        }
    }
    protected virtual void ApplyOnEndFightEffects()
    {
        foreach (var effect in _onEndFightEffects)
        {
            effect.ApplyOnEndFightEffect(this);
        }
    }
}
