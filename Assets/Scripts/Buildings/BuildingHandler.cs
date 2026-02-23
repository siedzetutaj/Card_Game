using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingHandler : InteractableObject, ITargetable
{
    public Transform TargetTransform => transform;
    public bool IsAlive => _health > 0;
    public bool IsAlly(bool isPlayerUnit) => isPlayerUnit == IsPlayerBuilding;
    public int TargetAmount
    {
        get => _targetAmount;
        set => _targetAmount = value;
    }

    public bool IsPlayerBuilding =true;
    
    [SerializeField] private Image _image;

    protected BuildingSO _buildingSO;
    protected int _health;
    private SelectedCard _selectedCard;

    private List<BuildingOnEndTurnEffectSO> _onEndTurnEffects = new();
    private List<BuildingOnEndFightEffectSO> _onEndFightEffects = new();

    private GameLogicManager _gameLogicManager => GameLogicManager.Instance;

    protected int _targetAmount;

    protected override void OnEnable()
    {
        base.OnEnable();

        _selectedCard = SelectedCard.Instance;
        _gameLogicManager.OnEndTurn += ApplyOnEndTurnEffects;
        _gameLogicManager.OnEndFight += ApplyOnEndFightEffects;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        _gameLogicManager.OnEndTurn -= ApplyOnEndTurnEffects;
        _gameLogicManager.OnEndFight -= ApplyOnEndFightEffects;

    }
    public virtual void Initialize(BuildingSO buildingSO)
    {
        _buildingSO = buildingSO;
        _image.sprite = buildingSO.Sprite;
        _health = buildingSO.health;
        _onEndTurnEffects = new (buildingSO.OnEndTurnEffects);
        _onEndFightEffects = new (buildingSO.OnEndFightEffects);
        if(IsPlayerBuilding)
            _gameLogicManager.PlayerBuildingsToTarget.Add(this);
        else
            _gameLogicManager.EnemieBuildingsToTarget.Add(this);
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
    public void TakeDamage(int damage, IAttacker attacker)
    {
        Debug.Log(_health);
        _health -= damage;
        if (!IsAlive)
            OnDeath(attacker);
    }

    public void OnDeath(IAttacker attacker)
    {
        attacker.OnKill();
        DestoryBuilding();
    }
    protected virtual void DestoryBuilding()
    {
        if(IsPlayerBuilding)
            _gameLogicManager.PlayerBuildingsToTarget.Remove(this);
        else
            _gameLogicManager.EnemieBuildingsToTarget.Remove(this);
        Destroy(gameObject);
    }
}
