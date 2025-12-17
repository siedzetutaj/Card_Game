using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBuildingHandler : BuildingHandler, IAttacker
{
    public int _damage;
    public int _range;
    public float _attackSpeed;

    private UnitHandler _targetUnit;
    private bool _canAttack = true;

    private float _retargetTimer;
    private float _retargetCooldown = 2f;

    public bool onEndTurn = false;  
    private GameLogicManager _gameLogicManager => GameLogicManager.Instance;
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
        if (onEndTurn)
        {
            Retarget(_gameLogicManager.EnemieUnits);
            _retargetTimer -= Time.fixedDeltaTime;

            if (_targetUnit == null) return;

            if (_canAttack)
                Attack();
        }
    }

    private void Retarget(List<UnitHandler> units)
    {
        if (_targetUnit == null || _retargetTimer <= 0f)
        {
            _targetUnit = FindBestTarget(units);
            _retargetTimer = _retargetCooldown;
            return;
        }
    }
    private UnitHandler FindBestTarget(List<UnitHandler> units)
    {
        if (units.Count == 0) return null;
        if (_targetUnit != null) _targetUnit.TargetAmount--;

        UnitHandler best = null;
        float bestScore = float.MinValue;

        foreach (UnitHandler unit in units)
        {
            if (unit == null) continue;

            float sqrDist = (unit.transform.position - transform.position).magnitude;

            float distanceScore = 1f / (sqrDist + 1f) * 100;   // closer = higher score
            float targetPenalty = unit.TargetAmount * 0.1f; // more targets = worse
            float randomness = Random.Range(0f, 0.02f);   // natural variation

            float score = distanceScore - targetPenalty + randomness;

            if (score > bestScore)
            {
                bestScore = score;
                best = unit;
            }
        }

        if (best != null)
            best.TargetAmount++;
        return best;
    }
    protected void Attack()
    {
        if (!_canAttack) return;

        _canAttack = false;
        _targetUnit.OnDecreaseHP(_damage, this);
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
        _targetUnit = null;
    }
}
