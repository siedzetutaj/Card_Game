using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseHandler : MonoBehaviour, ITargetable, IAttacker
{
    public int _damage = 9999;
    public int _range;
    public float _attackSpeed;

    protected ITargetable _currentTarget;
    protected bool _canAttack = true;

    protected float _retargetTimer;
    protected float _retargetCooldown = 2f;
    public bool IsUnit => false; 
    protected bool _isInRange = false;

    public Transform TargetTransform => transform;
    public bool IsAlive => true;
    public int TargetAmount
    {
        get => _targetAmount;
        set => _targetAmount = value;
    }
    public bool IsAlly(bool isPlayerUnit) => isPlayerUnit == _isPlayerBase;
    private int _targetAmount;
    protected TurnManager _turnManager => TurnManager.Instance;
    protected ResourceManager _resourceManager => ResourceManager.Instance;
    protected ResourceHandler _populationResourceHandler => _resourceManager.FindResource(ResourceType.population);


    [SerializeField] protected bool _isPlayerBase = true;

    protected void FixedUpdate()
    {
        if (_turnManager.CurrentPhase is GamePhaseCombat) 
        {
            Retarget();

            if (_currentTarget == null || _currentTarget.Equals(null)) 
                return;
            
            RangeCheck();

            if (_canAttack && _isInRange)
                Attack();
        }
    }
    protected virtual void Retarget()
    {

    }
    protected virtual ITargetable FindBestTarget(List<ITargetable> targets)
    {
        ITargetable best = null;
        float bestScore = float.MinValue;

        foreach (var target in targets)
        {
            if (!target.IsAlive) continue;
            if (!target.IsUnit) continue;

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
    protected void RangeCheck()
    {
        Vector2 distance = Vector2.Distance(transform.position, _currentTarget.TargetTransform.position) * Vector2.one;
        if (distance.magnitude > _range)
            _isInRange = false;
        else
            _isInRange = true;
    }
    protected void Attack()
    {
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
    public virtual void OnDeath(IAttacker attacker)
    {

    }
    public virtual void TakeDamage(int damage, IAttacker attacker)
    {

    }
}
