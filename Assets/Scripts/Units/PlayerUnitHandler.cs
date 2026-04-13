using System.Linq;
using UnityEngine;

public class PlayerUnitHandler : UnitHandler
{
    protected override void FixedUpdate()
    {
        if (_currentTarget == null || !_currentTarget.IsAlive)
            Retarget();
        base.FixedUpdate();
    }
    protected void Retarget()
    {
        _currentTarget = FindBestTarget(
            _turnManager.EnemieTargets.Cast<ITargetable>().ToList());

        Debug.Log($"Player unit retargeted to {_currentTarget}");
    }
}
