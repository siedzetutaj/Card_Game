using System.Linq;
using UnityEngine;

public class EnemieBaseHandler : BaseHandler
{
    public int health = 100;
    private EnemieManager _enemieManager=> EnemieManager.Instance;
    public new bool IsAlive => health > 0;
    private void Start()
    {
        _isPlayerBase = false;
        _gameLogicManager.EnemieBuildingsToTarget.Add(this);
        _enemieManager.HealthPoints = health;
    }
    protected override void Retarget()
    {
        if (_currentTarget == null || _retargetTimer <= 0f)
        {
            _currentTarget = FindBestTarget(_gameLogicManager.PlayerUnits.Cast<ITargetable>().ToList());
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
        health -= damage;
        _enemieManager.HealthPoints = health;
        if (!IsAlive)
            OnDeath(attacker);
    }
}
