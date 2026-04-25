using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SpawnUnit", menuName = "Scriptable Objects/Buildings/Effects/SpawnUnit")]
public class SpawnUnitBuildingEffectSO : BuildingEffectSO
{
    [SerializeField] private GameObject _playerUnitsManagerPrefab;
    [SerializeField] private GameObject _enemieUnitsManagerPrefab;
    [SerializeField] private UnitSO _unitSO;

    public override void ApplyBuildingEffect(BuildingHandler buildingHandler)
    {
        if (buildingHandler.IsPlayerBuilding)
        {

            var food = ResourceManager.Instance.FindResource(ResourceType.food);
            int foodCost = _unitSO.UnitData.UnitFoodCost;
            if (food.Amount >= foodCost)
            {
                food.DecreaseAmount(foodCost);
                SpawnUnits(buildingHandler);
            }
        }
        else
        {
            SpawnUnits(buildingHandler);
        }
    }
    private void SpawnUnits(BuildingHandler buildingHandler)
    {
        UnitData unitData = _unitSO.UnitData;

        Vector3 buildingPos = buildingHandler.transform.position;
        Vector3 spawnPoint = new Vector3(buildingPos.x + Random.Range(-10, 10), buildingPos.y + Random.Range(-10, 10), 0);
        if (buildingHandler.IsPlayerBuilding)
        {

            Transform parentTransform = PlayerUnitsManagers.Instance.transform;
            GameObject unitsManager = Instantiate(_playerUnitsManagerPrefab, parentTransform);  

            UnitsManager manager = unitsManager.GetComponent<UnitsManager>();
            manager.Initialize(unitData, spawnPoint, true);

            TurnManager.Instance.PlayerUnitsManagers.Add(manager);
        }
        else
        {
                Transform parentTransform = buildingHandler.transform;
                GameObject unitsManager = Instantiate(_enemieUnitsManagerPrefab, parentTransform);
    
                EnemieUnitsManager manager = unitsManager.GetComponent<EnemieUnitsManager>();
                manager.Initialize(unitData, spawnPoint, false);
    
                TurnManager.Instance.EnemieUnitsManagers.Add(manager);
        }
    }
}
