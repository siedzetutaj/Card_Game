using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseHandler : MonoBehaviour, ITargetable, IAttacker
{
    public int _damage = 9999;
    public int _range = 700;
    public float _attackSpeed = 0.5f;

    protected ITargetable _currentTarget;
    protected bool _canAttack = true;

    protected float _retargetTimer;
    protected float _retargetCooldown = 2f;

    public Transform TargetTransform => transform;
    public bool IsAlive => true;
    public int TargetAmount
    {
        get => _targetAmount;
        set => _targetAmount = value;
    }
    public bool IsAlly(bool isPlayerUnit) => isPlayerUnit == _isPlayerBase;
    private int _targetAmount;
    protected GameLogicManager _gameLogicManager => GameLogicManager.Instance;
    protected ResourceManager _resourceManager => ResourceManager.Instance;
    protected ResourceHandler _populationResourceHandler => _resourceManager.FindResource(ResourceType.population);

    [SerializeField] protected bool _isPlayerBase = true;

    protected void FixedUpdate()
    {
        if (_gameLogicManager.CurrentPhase == CombatPhase.Buildings)
        {
            Retarget();

            if (_currentTarget == null) return;

            if (_canAttack)
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
    public virtual void OnDeath(IAttacker attacker)
    {

    }
    public virtual void TakeDamage(int damage, IAttacker attacker)
    {

    }
}
