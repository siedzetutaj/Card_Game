using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootBuildingHandler : BuildingHandler, IAttacker
{
    public int _damage;
    public int _range;
    public float _attackSpeed;

    private ITargetable _currentTarget;
    private bool _canAttack = true;

    private float _retargetTimer;
    private float _retargetCooldown = 2f;
    private bool _isInRange;

    private TurnManager _turnManager => TurnManager.Instance;
    public override void Initialize(BuildingSO buildingSO)
    {
        base.Initialize(buildingSO);
        
        ShootBuildingSO shootBuildingSO = buildingSO as ShootBuildingSO;
        _damage = shootBuildingSO.Damage;
        _range = shootBuildingSO.Range;
        _attackSpeed = shootBuildingSO.AttackSpeed;
    }

    private void FixedUpdate()
    {
        if (_turnManager.CurrentPhase is GamePhaseCombat)
        {
            Retarget();

            if (_currentTarget == null || _currentTarget.Equals(null)) 
                return;

            if (_canAttack && RangeCheck(_currentTarget))
                Attack();
        }
    }

    private void Retarget()
    {
        if (_currentTarget == null || _retargetTimer <= 0f)
        {
            _currentTarget = FindBestTarget(_turnManager.EnemieTargets.Cast<ITargetable>().ToList());
            _retargetTimer = _retargetCooldown;
            return;
        }
    }
    protected bool RangeCheck(ITargetable target)
    {
        Vector2 distance = Vector2.Distance(transform.position, target.TargetTransform.position) * Vector2.one;
        if (distance.magnitude > _range)
            return false;
        else
            return true;
    }
    protected virtual ITargetable FindBestTarget(List<ITargetable> targets)
    {
        ITargetable best = null;
        float bestScore = float.MinValue;

        foreach (var target in targets)
        {
            if (!target.IsAlive) continue;
            if (!target.IsUnit) continue;
            if (RangeCheck(target)) continue;

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
}
