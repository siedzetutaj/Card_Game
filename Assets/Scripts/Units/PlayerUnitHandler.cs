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
        if (_gameLogicManager.CurrentPhase == CombatPhase.Units)
        {
            _currentTarget = FindBestTarget(
                _gameLogicManager.EnemieUnits.Cast<ITargetable>().ToList());
        }
        else if (_gameLogicManager.CurrentPhase == CombatPhase.Buildings)
        {
            _currentTarget = FindBestTarget(
                _gameLogicManager.EnemieBuildingsToTarget.Cast<ITargetable>().ToList());
        }
        Debug.Log($"Player unit retargeted to {_currentTarget}");
    }
}
