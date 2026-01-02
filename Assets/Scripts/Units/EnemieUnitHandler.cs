using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemieUnitHandler : UnitHandler
{
    protected override void FixedUpdate()
    {
        Retarget();
        base.FixedUpdate();
    }
    protected void Retarget()
    {
        if (_gameLogicManager.CurrentPhase == CombatPhase.Units)
        {
            _currentTarget = FindBestTarget(
                _gameLogicManager.PlayerUnits.Cast<ITargetable>().ToList());
        }
        else if (_gameLogicManager.CurrentPhase == CombatPhase.Buildings)
        {
            _currentTarget = FindBestTarget(
                _gameLogicManager.PlayerBuildingsToTarget.Cast<ITargetable>().ToList());
        }
    }
}
