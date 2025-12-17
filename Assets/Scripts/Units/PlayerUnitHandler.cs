using UnityEngine;

public class PlayerUnitHandler : UnitHandler
{
    protected override void FixedUpdate()
    {
        Retarget(_gameLogicManager.EnemieUnits);
        base.FixedUpdate();
    }
    protected override void FindTarget()
    {
        _targetUnit = FindBestTarget(_gameLogicManager.EnemieUnits);
    }
}
