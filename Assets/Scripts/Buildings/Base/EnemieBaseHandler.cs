using System.Linq;
using UnityEngine;

public class EnemieBaseHandler : BaseHandler
{
    private EnemieManager _enemieManager=> EnemieManager.Instance;
    private void Start()
    {
        _isPlayerBase = false;
        _turnManager.EnemieTargets.Add(this);
    }
    protected override void Retarget()
    {
        if ((Object)_currentTarget == null || _retargetTimer <= 0f)
        {
            _currentTarget = FindBestTarget(_turnManager.PlayerTargets.Cast<ITargetable>().ToList());
            _retargetTimer = _retargetCooldown;
            return;
        }
    }
    public override void OnDeath(IAttacker attacker)
    {
        Debug.Log("Enemie Base Destroyed! Player Wins!");
    }
    public override void TakeDamage(int damage, IAttacker attacker)
    {
        _enemieManager.HealthPoints -= damage;
    }
}
