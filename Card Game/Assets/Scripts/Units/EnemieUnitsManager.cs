using UnityEngine;

public class EnemieUnitsManager : UnitsManager
{

    public override void OnUnitDeath(UnitHandler unit)
    {
        Units.Remove(unit);
        if (Units.Count == 0)
        {
            GameLogicManager.Instance.OnFightEnd(false);
        }
    }
}
