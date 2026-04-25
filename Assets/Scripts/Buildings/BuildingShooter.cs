using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingShooter : MonoBehaviour, IAttacker
{
    private int _damage;
    private int _range;
    private float _attackSpeed;

    private ITargetable _currentTarget;
    private bool _canAttack = true;

    private float _retargetTimer;
    private float _retargetCooldown = 2f;
    private RangeMarker _rangeMarker;
    private BuildingHandler _buildingHandler;

    private TurnManager _turnManager => TurnManager.Instance;

    public void Setup(int damage, int range, float attackSpeed, BuildingHandler handler)
    {
        _damage = damage;
        _range = range;
        _attackSpeed = attackSpeed;
        _buildingHandler = handler;

        _rangeMarker = GetComponentInChildren<RangeMarker>(true);
        if (_rangeMarker != null)
        {
            _rangeMarker.SetRange(_range);
        }
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
            List<ITargetable> targets = _buildingHandler.IsPlayerBuilding ? 
                _turnManager.EnemieTargets.Cast<ITargetable>().ToList() : 
                _turnManager.PlayerTargets.Cast<ITargetable>().ToList();

            _currentTarget = FindBestTarget(targets);
            _retargetTimer = _retargetCooldown;
            return;
        }
        _retargetTimer -= Time.fixedDeltaTime;
    }

    protected bool RangeCheck(ITargetable target)
    {
        Vector2 distance = Vector2.Distance(transform.position, target.TargetTransform.position) * Vector2.one;
        return distance.magnitude <= _range;
    }

    protected virtual ITargetable FindBestTarget(List<ITargetable> targets)
    {
        ITargetable best = null;
        float bestScore = float.MinValue;

        foreach (var target in targets)
        {
            if (!target.IsAlive) continue;
            if (!target.IsUnit) continue;
            if (!RangeCheck(target)) continue;

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
        yield return new WaitForSeconds(_attackSpeed);
        _canAttack = true;
    }

    public void OnKill()
    {
        _currentTarget = null;
    }

    // These were in ShootBuildingHandler to enable/disable range marker
    public void EnableRangeMarker()
    {
        if (_rangeMarker == null) _rangeMarker = GetComponentInChildren<RangeMarker>(true);
        if (_rangeMarker != null) _rangeMarker.EnableVisiblity();
    }

    public void DisableRangeMarker()
    {
        if (_rangeMarker == null) _rangeMarker = GetComponentInChildren<RangeMarker>(true);
        if (_rangeMarker != null) _rangeMarker.DisableVisiblity();
    }
}
