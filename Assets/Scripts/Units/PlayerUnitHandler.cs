using System.Linq;
using UnityEngine;

public class PlayerUnitHandler : UnitHandler
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
                _gameLogicManager.EnemieUnits.Cast<ITargetable>().ToList());
        }
        else
        {
            //_currentTarget = FindBestTarget(
            //    _gameLogicManager.PlayerBuildings.Cast<ITargetable>().ToList());
            Debug.Log("No Enemie Buildings");
        }
    }
}
