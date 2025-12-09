using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    protected UnitData _unitData;
    protected UnitHandler _targetUnit;

    protected bool _canAttack = true;
    protected float _waitBetweenAttack = 1f;

    protected bool _isPlayerUnit;
    protected UnitsManager _unitsManager;

    protected GameLogicManager _gameLogicManager => GameLogicManager.Instance;
    public void Inititalize(UnitData unitData, bool isPlayerUnit, UnitsManager unitsManager)
    {
        _unitData = new UnitData(unitData);
        _spriteRenderer.sprite = unitData.UnitSprite;
        _waitBetweenAttack =  unitData.UnitAttackSpeed;
        _isPlayerUnit = isPlayerUnit;
        _unitsManager = unitsManager;
        transform.position = new Vector3(transform.position.x + Random.Range(-20,20),
            transform.position.y + Random.Range(-20, 20), -200);
    }
    protected void FixedUpdate()
    {
        if(_targetUnit == null)
            FindTarget();
        else
        {
            RangeCheck();
            if(!_canAttack)
                Movement();
            else
                Attack();
        }
    }

    protected void Movement()
    {
        Vector3 targetTransform = _targetUnit.transform.position;  
        Vector3 direction = (targetTransform - this.transform.position);
        if (direction.sqrMagnitude <= 0.005f)
        {
            transform.position = targetTransform;
            return;
        }
        transform.position += direction.normalized * _unitData.UnitSpeed * Time.fixedDeltaTime;
    }
    protected virtual void FindTarget()
    {

    }
    protected UnitHandler FindBestTarget(List<UnitHandler> units)
    {
        if (units.Count <= 0) return null;
        UnitHandler best = null;
        float bestSqrDist = float.MaxValue;

        foreach (UnitHandler unit in units)
        {
            if (unit == null) continue;
            float sqrDist = (unit.gameObject.transform.position - transform.position).sqrMagnitude;
            if (sqrDist < bestSqrDist)
            {
                bestSqrDist = sqrDist;
                best = unit;
            }
        }
        return best;
    }
    protected void RangeCheck()
    {
        Vector2 distance = Vector2.Distance(transform.position, _targetUnit.transform.position) * Vector2.one;
        if (distance.magnitude > _unitData.UnitAttackRange)
            _canAttack = false;
        else
            _canAttack = true;
    }
    protected void Attack()
    {
        _targetUnit.OnDecreaseHP(_unitData.UnitDamage, this);
        StartCoroutine(WaitCoroutine());
    }
    public void OnDecreaseHP(int damage, UnitHandler attacker)
    {
        _unitData.UnitHealth -= damage;
        if (_unitData.UnitHealth <= 0) 
            OnDeath(attacker);
    }
    protected void OnDeath(UnitHandler attacker)
    {
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
        _targetUnit = null;
    }
    IEnumerator WaitCoroutine()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_waitBetweenAttack);
        _canAttack = true;
    }
}
