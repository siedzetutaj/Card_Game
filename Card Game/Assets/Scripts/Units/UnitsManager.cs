using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    public Action OnUnitDeath;
    
    [SerializeField] private GameObject _unitPrefab;
    
    private UnitData _unitData;
    public List<UnitHandler> Units = new();

    public void Initialize(UnitData unitData, Transform spawnPoint, bool isPlayerUnit)
    {
        _unitData = new UnitData(unitData);

        CreateUnits(isPlayerUnit);
    }
    private void OnEnable()
    {
        OnUnitDeath += UnitDeath;
    }
    private void OnDisable()
    {
        OnUnitDeath -= UnitDeath;
    }
    private void CreateUnits(bool isPlayerUnit)
    {
        //TODO: Sekwencyjne ukladanie jednostek w formacje
        for (int i =0; i< _unitData.UnitAmount; i++)
        {
            GameObject unit = Instantiate(_unitPrefab, transform);
            unit.GetComponent<UnitHandler>().Inititalize(_unitData, isPlayerUnit);
            Units.Add(unit.GetComponent<UnitHandler>());
        }
    }   
    private void UnitDeath()
    {

    }
}
