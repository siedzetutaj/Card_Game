using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemieUnitsManager : UnitsManager
{
    public UnitSO UnitSO;
    [NonSerialized] public UnitData UnitData;
    public List<EnemieUnitHandler> EnemieUnits;
    private void OnEnable()
    {
        UnitData = new UnitData(UnitSO.UnitData);
    }
    private void OnDestroy()
    {
        TurnManager turnManager = TurnManager.Instance;
        if (turnManager == null) return;
        turnManager.EnemieUnitsManagers.Remove(this);
    }
    public override void OnUnitDeath(UnitHandler unit)
    {
        var Enemieunit = unit as EnemieUnitHandler;
        EnemieUnits.Remove(Enemieunit);
        if (EnemieUnits.Count == 0)
        {
            var food = EnemieResourceManager.Instance.FindResource(ResourceType.food);
            if (food.Amount > 0) return;
            TurnManager turnManager = TurnManager.Instance;
            turnManager.EnemieUnitsManagers.Remove(this);

            Destroy(gameObject);
        }
    }
    protected override void CreateUnit(bool isPlayerUnit)
    {
        var food = EnemieResourceManager.Instance.FindResource(ResourceType.food);
        food.DecreaseAmount(UnitData.UnitFoodCost);
        GameObject unit = Instantiate(_unitPrefab, transform);
        var unitHandler = unit.GetComponent<UnitHandler>();
        unitHandler.Inititalize(_unitData, isPlayerUnit, this);
        EnemieUnits.Add(unit.GetComponent<EnemieUnitHandler>());
        TurnManager.Instance.EnemieTargets.Add(unitHandler);
    }
}
