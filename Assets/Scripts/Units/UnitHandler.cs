using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class UnitHandler : MonoBehaviour, IAttacker, ITargetable
{
    public Transform TargetTransform => transform;
    public bool IsAlive => _unitData.UnitHealth > 0;
    protected ITargetable _currentTarget;
    public bool IsUnit => true;
    public int TargetAmount
    {
        get => _targetAmount;
        set => _targetAmount = value;
    }

    [SerializeField] protected SpriteRenderer _spriteRenderer;
    protected UnitData _unitData;

    protected int _unitAttackRange;
    protected bool _isInRange = false;
    protected bool _canAttack = true;
    protected float _waitBetweenAttack;

    protected bool _isPlayerUnit;
    protected UnitsManager _unitsManager;
    protected GameLogicManager _gameLogicManager => GameLogicManager.Instance;


    protected int _targetAmount;
    public void Inititalize(UnitData unitData, bool isPlayerUnit, UnitsManager unitsManager)
    {
        _unitData = new UnitData(unitData);
        _spriteRenderer.sprite = unitData.UnitSprite;
        _waitBetweenAttack = unitData.UnitAttackSpeed;
        _isPlayerUnit = isPlayerUnit;
        _unitsManager = unitsManager;
        _unitAttackRange = unitData.UnitAttackRange;
        transform.position = new Vector3(transform.position.x + Random.Range(-20, 20),
        transform.position.y + Random.Range(-20, 20), -200);
        _targetAmount = -unitData.TargetAmount;
    }
    protected virtual void FixedUpdate()
    {
        if (_currentTarget == null) return;

        RangeCheck();

        if (!_isInRange)
            Movement();
        else
            Attack();
    }
    //protected virtual void Retarget(List<UnitHandler> units)
    //{
    //    if (_targetUnit == null || _retargetTimer <= 0f)
    //    {
    //        _targetUnit = FindBestTarget(units);
    //        _retargetTimer = _retargetCooldown;
    //        return;
    //    }
    //}
    protected void Movement()
    {
        Vector3 targetPos = _currentTarget.TargetTransform.position;
        Vector3 directionToTarget = (targetPos - transform.position);

        Vector3 separation = CalculateSeparation();

        Vector3 finalDirection = (directionToTarget.normalized + separation * 0.5f).normalized;

        if (directionToTarget.sqrMagnitude <= _unitAttackRange)
        {
            transform.position = targetPos;
            return;
        }

        transform.position += finalDirection * _unitData.UnitSpeed * Time.fixedDeltaTime;
    }

    private Vector3 CalculateSeparation()
    {
        Vector3 separationVec = Vector3.zero;
        float desiredSeparation = 100f; 

        Collider2D[] neighbors = Physics2D.OverlapCircleAll(transform.position, desiredSeparation);

        foreach (var neighbor in neighbors)
        {
            if (neighbor.gameObject != this.gameObject)
            {
                Vector3 diff = transform.position - neighbor.transform.position;
                separationVec += diff.normalized / diff.magnitude;
            }
        }

        return separationVec;
    }
    protected ITargetable FindBestTarget(List<ITargetable> targets)
    {
        if (targets.Count == 0) return null;
        if (_currentTarget != null) _currentTarget.TargetAmount--;
        ITargetable best = null; float bestScore = float.MinValue;
        foreach (ITargetable target in targets)
        {
            if (target == null) continue;

            float sqrDist = (target.TargetTransform.position - transform.position).magnitude;
            float distanceScore = 1f / (sqrDist + 1f) * 100; // closer = higher score 
            float targetPenalty = target.TargetAmount * 0.1f; // more targets = worse 

            float randomness = Random.Range(0f, 0.02f); // natural variation 
            float score = distanceScore - targetPenalty + randomness;

            if (score > bestScore)
            {
                bestScore = score;
                best = target;
            }
        }
        if (best != null)
        {
            best.TargetAmount++;

            if (best is UnitHandler bestUnit && bestUnit._currentTarget == null)
            {
                bestUnit._currentTarget = this;
            }
        }
        if (best != null)
            best.TargetAmount++;
        return best;
    }
    protected void RangeCheck()
    {
        Vector2 distance = Vector2.Distance(transform.position, _currentTarget.TargetTransform.position) * Vector2.one;
        if (distance.magnitude > _unitData.UnitAttackRange)
            _isInRange = false;
        else
            _isInRange = true;
    }
    protected void Attack()
    {
        if (!_canAttack || _currentTarget == null) return;

        _canAttack = false;
        _currentTarget.TakeDamage(_unitData.UnitDamage, this);
        StartCoroutine(WaitCoroutine());
    }
    public void TakeDamage(int damage, IAttacker attacker)
    {
        _unitData.UnitHealth -= damage;
        if (_unitData.UnitHealth <= 0)
            OnDeath(attacker);
    }
    public void OnDeath(IAttacker attacker)
    {
        if (_currentTarget != null)
            _currentTarget.TargetAmount--;
        if(_isPlayerUnit)
            _gameLogicManager.PlayerTargets.Remove(this);
        else
            _gameLogicManager.EnemieTargets.Remove(this as EnemieUnitHandler);
        attacker.OnKill();
        DestroyUnit();
    }
    public void DestroyUnit()
    {
        _unitsManager.OnUnitDeath(this);
        Destroy(gameObject);
    }
    public bool IsAlly(bool isPlayerUnit)
    {
        return isPlayerUnit == _isPlayerUnit;
    }
    public void OnKill()
    {
        _currentTarget = null;
    }
    public void Retaliation(ITargetable targetToRetaliate)
    {
        if (targetToRetaliate != null && !_isInRange)
        {
            _currentTarget.TargetAmount--;
            _currentTarget = targetToRetaliate;
            _currentTarget.TargetAmount++;
        }
    }
    IEnumerator WaitCoroutine()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_waitBetweenAttack);
        _canAttack = true;
    }
}
