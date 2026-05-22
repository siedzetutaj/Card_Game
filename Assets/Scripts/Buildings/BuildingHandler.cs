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
    public bool IsUnit => false;

    public bool IsPlayerBuilding =true;
    public int Health => _health;
    public event System.Action<int> OnHealthChanged;

    [SerializeField] private Image _image;

    [SerializeField] protected BuildingSO _buildingSO;
    [SerializeField] protected int _health;
    private SelectedCard _selectedCard;

    private List<BuildingEffectSO> _onBeginningTacticalEffects = new();
    private List<BuildingEffectSO> _onEndTacticalEffects = new();

    private List<BuildingEffectSO> _onBeginningCombatEffects = new();
    private List<BuildingEffectSO> _onEndCombatEffects = new();

    private List<BuildingEffectSO> _onDeathEffects = new();

    private TurnManager _turnManager => TurnManager.Instance;

    protected int _targetAmount;
    private bool _initialized;

    protected virtual void Start()
    {
        if (!_initialized && _buildingSO != null)
        {
            Initialize(_buildingSO);
        }
    }


    protected override void OnEnable()
    {
        base.OnEnable();

        _selectedCard = SelectedCard.Instance;
        _turnManager.OnTacticalPhaseStart += ApplyOnBeginningTacticalEffects;
        _turnManager.OnTacticalPhaseEnd += ApplyOnEndTacticalEffects;
        
        _turnManager.OnCombatPhaseStart += ApplyOnBeginningCombatEffects;
        _turnManager.OnCombatPhaseEnd += ApplyOnEndCombatEffects;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        _turnManager.OnTacticalPhaseStart -= ApplyOnBeginningTacticalEffects;
        _turnManager.OnTacticalPhaseEnd -= ApplyOnEndTacticalEffects;

        _turnManager.OnCombatPhaseStart -= ApplyOnBeginningCombatEffects;
        _turnManager.OnCombatPhaseEnd -= ApplyOnEndCombatEffects;

    }
    public virtual void Initialize(BuildingSO buildingSO)
    {
        _initialized = true;
        _buildingSO = buildingSO;
        _image.sprite = buildingSO.Sprite;
        _health = buildingSO.health;

        _onBeginningTacticalEffects = new (buildingSO.OnBeginningTacticalEffects);
        _onEndTacticalEffects = new (buildingSO.OnEndTacticalEffects);

        _onBeginningCombatEffects = new (buildingSO.OnBeginningCombatEffects);
        _onEndCombatEffects = new (buildingSO.OnEndCombatEffects);

        _onDeathEffects = new (buildingSO.OnDeathEffects);

        foreach (var effect in buildingSO.OnBeginningBuildingEffects)
        {
            effect.ApplyBuildingEffect(this);
        }
    }
    protected override void OnObjectClicked()
    {
        base.OnObjectClicked();

        bool canCardBePlayed = (_selectedCard.Card != null &&
            _selectedCard.Card.CardType == CardType.Recruitment);

        if (canCardBePlayed)
        {
            CardHandler card = SelectedCard.Instance.Card;
            card.OnCardPlayed(gameObject);
        }

        bool canBuildingBeUpgraded = (_selectedCard.Card != null &&
            _selectedCard.Card.CardType == CardType.Building &&
            _buildingSO.NextLevelPrefab != null);

        if (canBuildingBeUpgraded)
        {
            BuildingCardEffectSO cardEffect = null;
            foreach (var effect in _selectedCard.Card.CardData.Effects)
            {
                if (effect is BuildingCardEffectSO bce)
                {
                    cardEffect = bce;
                    break;
                }
            }
            
            if (cardEffect != null)
            {
                BuildingSO cardBuildingSO = cardEffect.BuildingSO;
                BuildingSO myBase = _buildingSO.BaseBuildingSO != null ? _buildingSO.BaseBuildingSO : _buildingSO;
                BuildingSO cardBase = cardBuildingSO.BaseBuildingSO != null ? cardBuildingSO.BaseBuildingSO : cardBuildingSO;
                
                if (myBase == cardBase)
                {
                    CardHandler card = SelectedCard.Instance.Card;
                    card.OnCardPlayed(gameObject);
                }
            }
        }

        if (TryGetComponent<BuildingShooter>(out var shooter))
        {
            shooter.EnableRangeMarker();
        }
    }
    protected override void OnObjectMouseExit()
    {
        base.OnObjectMouseExit();
        if (TryGetComponent<BuildingShooter>(out var shooter))
        {
            shooter.DisableRangeMarker();
        }
    }
    protected virtual void ApplyOnBeginningTacticalEffects()
    {
        foreach (var effect in _onBeginningTacticalEffects)
        {
            effect.ApplyBuildingEffect(this);
        }
    }
    protected virtual void ApplyOnEndTacticalEffects()
    {
        foreach (var effect in _onEndTacticalEffects)
        {
            effect.ApplyBuildingEffect(this);
        }
    }
    protected virtual void ApplyOnBeginningCombatEffects()
    {
        if (IsPlayerBuilding)
            _turnManager.PlayerTargets.Add(this);   
        else
            _turnManager.EnemieTargets.Add(this);

        foreach (var effect in _onBeginningCombatEffects)
        {
            effect.ApplyBuildingEffect(this);
        }
    }
    protected virtual void ApplyOnEndCombatEffects()
    {
        foreach (var effect in _onEndCombatEffects)
        {
            effect.ApplyBuildingEffect(this);
        }
    }
    protected virtual void ApplyOnDeathEffects()
    {
        foreach (var effect in _onDeathEffects)
        {
            effect.ApplyBuildingEffect(this);
        }
    }
    public void TakeDamage(int damage, IAttacker attacker)
    {
        Debug.Log(_health);
        _health -= damage;
        OnHealthChanged?.Invoke(_health);
        if (!IsAlive)
            OnDeath(attacker);
    }

    public void OnDeath(IAttacker attacker)
    {
        ApplyOnDeathEffects();
        attacker.OnKill();
        DestoryBuilding();
    }
    protected virtual void DestoryBuilding()
    {
        if(IsPlayerBuilding)
            _turnManager.PlayerTargets.Remove(this);
        else
            _turnManager.EnemieTargets.Remove(this);
        Destroy(gameObject);
    }

    public void Upgrade()
    {
        if (_buildingSO.NextLevelPrefab == null) return;

        _buildingSO = _buildingSO.NextLevelPrefab;
        
        if (_buildingSO.Sprite != null)
        {
            _image.sprite = _buildingSO.Sprite;
        }
        
        _health = _buildingSO.health; 

        foreach (var effect in _buildingSO.OnBeginningBuildingEffects)
        {
            effect.ApplyBuildingEffect(this);
        }
        
        _onBeginningTacticalEffects = new(_buildingSO.OnBeginningTacticalEffects);
        _onEndTacticalEffects = new(_buildingSO.OnEndTacticalEffects);
        _onBeginningCombatEffects = new(_buildingSO.OnBeginningCombatEffects);
        _onEndCombatEffects = new(_buildingSO.OnEndCombatEffects);
        _onDeathEffects = new(_buildingSO.OnDeathEffects);
    }
}
