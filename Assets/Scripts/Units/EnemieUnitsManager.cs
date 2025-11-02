using UnityEngine;

public class EnemieUnitsManager : UnitsManager
{
    public UnitSO UnitSO;
    public UnitData UnitData;

    private void OnEnable()
    {
        UnitData = new UnitData(UnitSO.UnitData);
    }
    public override void OnUnitDeath(UnitHandler unit)
    {
        Units.Remove(unit);
        if (Units.Count == 0)
        {
            GameLogicManager.Instance.OnFightEnd(false);
        }
    }
}
