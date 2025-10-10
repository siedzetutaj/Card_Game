using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    [SerializeField] private GameObject _unitPrefab;
    
    private UnitData _unitData;
    public List<UnitHandler> Units = new();

    public void Initialize(UnitData unitData, Transform spawnPoint, bool isPlayerUnit)
    {
        _unitData = new UnitData(unitData);

        CreateUnits(isPlayerUnit);
    }
    private void CreateUnits(bool isPlayerUnit)
    {
        //TODO: Sekwencyjne ukladanie jednostek w formacje
        for (int i =0; i< _unitData.UnitAmount; i++)
        {
            GameObject unit = Instantiate(_unitPrefab, transform);
            unit.GetComponent<UnitHandler>().Inititalize(_unitData, isPlayerUnit,this);
            Units.Add(unit.GetComponent<UnitHandler>());
        }
    }   
    public virtual void OnUnitDeath(UnitHandler unit)
    {
        Units.Remove(unit);
        if(Units.Count == 0)
        {
            GameLogicManager gameLogicManager = GameLogicManager.Instance;
            gameLogicManager.PlayerUnitsManagers.Remove(this);

            if(gameLogicManager.PlayerUnitsManagers.Count == 0)
                gameLogicManager.OnFightEnd();

            Destroy(gameObject);   
        }
    }
}
