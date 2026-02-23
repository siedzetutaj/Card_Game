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
            GameLogicManager gameLogicManager = GameLogicManager.Instance;
            gameLogicManager.EnemieUnitsManagers.Remove(this);

            if (gameLogicManager.EnemieUnitsManagers.Count == 0)
                gameLogicManager.OnFightEnd(true);

            Destroy(gameObject);
        }
    }
    protected override void CreateUnits(bool isPlayerUnit)
    {
        for (int i = 0; i < _unitData.UnitAmount; i++)
        {
            GameObject unit = Instantiate(_unitPrefab, transform);
            unit.GetComponent<EnemieUnitHandler>().Inititalize(_unitData, isPlayerUnit, this);
            EnemieUnits.Add(unit.GetComponent<EnemieUnitHandler>());
        }
    }
}
