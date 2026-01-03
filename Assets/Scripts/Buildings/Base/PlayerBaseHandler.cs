using System.Linq;
using UnityEngine;

public class PlayerBaseHandler : BaseHandler
{
    public new bool  IsAlive => _populationResourceHandler.Amount > 0;
    private void Start()
    {
        _gameLogicManager.PlayerBuildingsToTarget.Add(this);
    }
    protected override void Retarget()
    {
        if (_currentTarget == null || _retargetTimer <= 0f)
        {
            _currentTarget = FindBestTarget(_gameLogicManager.EnemieUnits.Cast<ITargetable>().ToList());
            _retargetTimer = _retargetCooldown;
            return;
        }
    }
    public override void OnDeath(IAttacker attacker)
    {
        Debug.Log("Game Over");
    }
    public override void TakeDamage(int damage, IAttacker attacker)
    {
        _populationResourceHandler.DecreaseAmount(damage);
        Debug.Log($"Player Base took {damage} damage");
        if (!IsAlive)
            OnDeath(attacker);
    }
}
