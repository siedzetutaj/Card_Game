using NUnit.Framework.Internal;
using System.Collections;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private UnitData _unitData;
    private UnitHandler _targetUnit;
    
    private bool _canAttack = true;
    private float _waitBetweenAttack = 1f;

    private bool _isPlayerUnit;

    private GameLogicManager _gameLogicManager => GameLogicManager.Instance;
    public void Inititalize(UnitData unitData, bool isPlayerUnit)
    {
        _unitData = new UnitData(unitData);
        _spriteRenderer.sprite = unitData.UnitSprite;
        _waitBetweenAttack =  unitData.UnitAttackSpeed;
        _isPlayerUnit = isPlayerUnit;
        transform.position = new Vector3(transform.position.x + Random.Range(-20,20),
            transform.position.y + Random.Range(-20, 20), 0);
    }
    private void FixedUpdate()
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

    private void Movement()
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
    private void FindTarget()
    {
        if (_isPlayerUnit)
            _targetUnit = FindBestTarget(_gameLogicManager.EnemieUnits);
        else
            _targetUnit = FindBestTarget(_gameLogicManager.PlayerUnits);
    }
    private UnitHandler FindBestTarget(UnitsManager unitsManager)
    {
        if(!unitsManager) return null;
        UnitHandler best = null;
        float bestSqrDist = float.MaxValue;

        foreach (UnitHandler unit in unitsManager.Units)
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
    private void RangeCheck()
    {
        Vector2 distance = Vector2.Distance(transform.position, _targetUnit.transform.position) * Vector2.one;
        if (distance.magnitude > _unitData.UnitAttackRange)
            _canAttack = false;
        else
            _canAttack = true;
    }
    private void Attack()
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
    private void OnDeath(UnitHandler attacker)
    {
        attacker.OnKill();
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
