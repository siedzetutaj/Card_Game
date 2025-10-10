using UnityEngine;

public class PlayerUnitHandler : UnitHandler
{
    protected override void FindTarget()
    {
        _targetUnit = FindBestTarget(_gameLogicManager.EnemieUnits);
    }
}
