using System.Linq;
using UnityEngine;

public class EnemieUnitHandler : UnitHandler
{
    protected override void FixedUpdate()
    {
        if(_currentTarget == null || !_currentTarget.IsAlive)  
            Retarget();
        base.FixedUpdate();
    }
    protected void Retarget()
    {
        _currentTarget = FindBestTarget(
            _turnManager.PlayerTargets.Cast<ITargetable>().ToList());
        Debug.Log($"Enemy unit retargeted to {_currentTarget}");

    }
}
