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
    public override void OnUnitDeath(UnitHandler unit)
    {
        var Enemieunit = unit as EnemieUnitHandler;
        EnemieUnits.Remove(Enemieunit);
        if (EnemieUnits.Count == 0)
        {
            var food = EnemieResourceManager.Instance.FindResource(ResourceType.food);
            if (food.Amount > 0) return;
            GameLogicManager gameLogicManager = GameLogicManager.Instance;
            gameLogicManager.EnemieUnitsManagers.Remove(this);

            if (gameLogicManager.EnemieUnitsManagers.Count == 0)
                gameLogicManager.OnFightEnd(true);

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
        _gameLogicManager.EnemieTargets.Add(unitHandler);
    }
}
