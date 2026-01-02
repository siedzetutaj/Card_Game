using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseHandler : MonoBehaviour, ITargetable, IAttacker
{
    public int _damage = 9999;
    public int _range = 700;
    public float _attackSpeed = 0.5f;

    private ITargetable _currentTarget;
    private bool _canAttack = true;

    private float _retargetTimer;
    private float _retargetCooldown = 2f;

    public Transform TargetTransform => transform;
    public bool IsAlive
    {
        get
        {
            if(_isPlayerBase)
                return _populationResourceHandler.Amount > 0;
            else
                return health > 0;
        }
    }
    public int TargetAmount
    {
        get => _targetAmount;
        set => _targetAmount = value;
    }
    public bool IsAlly(bool isPlayerUnit) => isPlayerUnit == _isPlayerBase;
    private int _targetAmount;
    private GameLogicManager _gameLogicManager => GameLogicManager.Instance;
    private ResourceManager _resourceManager => ResourceManager.Instance;
    private ResourceHandler _populationResourceHandler => _resourceManager.FindResource(ResourceType.population);

    [SerializeField] private bool _isPlayerBase = true;

    //TO DO: Rework to inheritance 
    public int health = 100;
    private void Start()
    {
        if (_isPlayerBase)
            _gameLogicManager.PlayerBuildingsToTarget.Add(this);
        else
            _gameLogicManager.EnemieBuidingsToTarget.Add(this);
    }
    private void FixedUpdate()
    {
        if (_gameLogicManager.CurrentPhase == CombatPhase.Buildings)
        {
            Retarget();

            if (_currentTarget == null) return;

            if (_canAttack)
                Attack();
        }
    }
    private void Retarget()
    {
        if (_isPlayerBase) 
        {
            if (_currentTarget == null || _retargetTimer <= 0f)
            {
                _currentTarget = FindBestTarget(_gameLogicManager.EnemieUnits.Cast<ITargetable>().ToList());
                _retargetTimer = _retargetCooldown;
                return;
            }
        }
        else
        {
            if (_currentTarget == null || _retargetTimer <= 0f)
            {
                _currentTarget = FindBestTarget(_gameLogicManager.PlayerUnits.Cast<ITargetable>().ToList());
                _retargetTimer = _retargetCooldown;
                return;
            }
        }
    }
    protected virtual ITargetable FindBestTarget(List<ITargetable> targets)
    {
        ITargetable best = null;
        float bestScore = float.MinValue;

        foreach (var target in targets)
        {
            if (!target.IsAlive) continue;

            float dist = Vector3.Distance(transform.position, target.TargetTransform.position);
            float score = 1f / (dist + 1f);

            if (score > bestScore)
            {
                bestScore = score;
                best = target;
            }
        }

        return best;
    }
    protected void Attack()
    {
        if (!_canAttack) return;

        _canAttack = false;
        _currentTarget.TakeDamage(_damage, this);
        StartCoroutine(WaitCoroutine());
    }

    IEnumerator WaitCoroutine()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackSpeed);
        _canAttack = true;
    }
    public void OnKill()
    {
        _currentTarget = null;
    }
    public void OnDeath(IAttacker attacker)
    {
        Debug.Log("Game Over");
    }
    public void TakeDamage(int damage, IAttacker attacker)
    {
        if(_isPlayerBase)
            _populationResourceHandler.DecreaseAmount(damage);
        else
            health -= damage;
        
        if(!IsAlive)
            OnDeath(attacker);
    }


}
