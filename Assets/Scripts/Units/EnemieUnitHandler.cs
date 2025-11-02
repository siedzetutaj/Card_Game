using UnityEngine;

public class EnemieUnitHandler : UnitHandler
{
    protected override void FindTarget()
    {
        _targetUnit = FindBestTarget(_gameLogicManager.PlayerUnits);

    }
}
