using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    [SerializeField] protected GameObject _unitPrefab;
    
    protected UnitData _unitData;
    public List<UnitHandler> Units = new();
    public TurnManager _turnManager => TurnManager.Instance;

    public void Initialize(UnitData unitData, Vector3 spawnPoint, bool isPlayerUnit)
    {
        _unitData = new UnitData(unitData);
        transform.position = spawnPoint;
        StartCoroutine(SpawnUnits(isPlayerUnit));
    }
    protected virtual void CreateUnit(bool isPlayerUnit)
    {
        var food = ResourceManager.Instance.FindResource(ResourceType.food);
        food.DecreaseAmount(_unitData.UnitFoodCost);
        GameObject unit = Instantiate(_unitPrefab, transform);
        var unitHandler = unit.GetComponent<UnitHandler>();
        unitHandler.Inititalize(_unitData, isPlayerUnit, this);
        Units.Add(unit.GetComponent<UnitHandler>());
        _turnManager.PlayerTargets.Add(unitHandler);
    }
    public virtual void OnUnitDeath(UnitHandler unit)
    {
        Units.Remove(unit);
        if (Units.Count == 0)
        {
            var food = ResourceManager.Instance.FindResource(ResourceType.food);
            if (food.Amount > 0) return;
            TurnManager turnManager = TurnManager.Instance;
            turnManager.PlayerUnitsManagers.Remove(this);

            Destroy(gameObject);
        }
    }
    
    IEnumerator SpawnUnits( bool isPlayerUnit)
    {
        var food = ResourceManager.Instance.FindResource(ResourceType.food);
        while (food.Amount >= _unitData.UnitFoodCost)
        {
            CreateUnit(isPlayerUnit);
            yield return new WaitForSeconds(_unitData.SpawnAfterSeconds);
        }
    }
}
