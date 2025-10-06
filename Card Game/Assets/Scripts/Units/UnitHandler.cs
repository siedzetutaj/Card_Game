using System.Collections;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private UnitData _unitData;
    private UnitHandler _targetUnit;
    private bool _canAttack = true;
    private float _waitBetweenAttack = 1f;
    public void Inititalize(UnitData unitData)
    {
        _unitData = new UnitData(unitData);
        _spriteRenderer.sprite = unitData.UnitSprite;
        _waitBetweenAttack = 1f / unitData.UnitAttackSpeed;
    }
    private void FixedUpdate()
    {
        FindTarget();
        Movement();
        RangeCheck();
        Attack();
    }

    private void Movement()
    {
        //TODO:
        Debug.Log("Movement");
    }
    private void FindTarget()
    {

    }
    private void RangeCheck()
    {
        if(_targetUnit == null) return;
        
        Vector2 distance = Vector2.Distance(transform.position, _targetUnit.transform.position) * Vector2.one;
        Debug.Log(distance);

        if(distance.magnitude > _unitData.UnitAttackRange)
            _targetUnit = null;
    }
    private void Attack()
    {
        if(!_canAttack) return;
        if (_targetUnit == null) return;

        _targetUnit.OnDecreaseHP(_unitData.UnitAttackSpeed);
        StartCoroutine(WaitCoroutine());
    }
    public void OnDecreaseHP(int damage)
    {
        _unitData.UnitHealth -= damage;
        if (_unitData.UnitHealth <= 0) 
            OnDeath();
    }
    private void OnDeath()
    {
        //TODO:
        Debug.Log("Death");
    }

    IEnumerator WaitCoroutine()
    {
        _canAttack = false;
        yield return null;
        //animator.ResetTrigger("attack");
        yield return new WaitForSeconds(_waitBetweenAttack);
        _canAttack = true;
    }
}
