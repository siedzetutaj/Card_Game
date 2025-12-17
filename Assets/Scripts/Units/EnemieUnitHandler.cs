using Unity.VisualScripting;
using UnityEngine;

public class EnemieUnitHandler : UnitHandler
{
    protected override void FixedUpdate()
    {
        Retarget(_gameLogicManager.PlayerUnits);
        base.FixedUpdate();
    }
    protected override void FindTarget()
    {
        _targetUnit = FindBestTarget(_gameLogicManager.PlayerUnits);
    }
}
