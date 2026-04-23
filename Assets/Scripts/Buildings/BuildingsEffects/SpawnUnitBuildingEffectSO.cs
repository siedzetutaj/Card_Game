using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SpawnUnit", menuName = "Scriptable Objects/Buildings/Effects/SpawnUnit")]
public class SpawnUnitBuildingEffectSO : BuildingEffectSO
{
    [SerializeField] private GameObject _playerUnitsManagerPrefab;
    [SerializeField] private GameObject _enemieUnitsManagerPrefab;

    public override void ApplyBuildingEffect(BuildingHandler buildingHandler)
    {
        SpawnBuildingHandler spawnBuildingHandler = buildingHandler as SpawnBuildingHandler;
        
        if (buildingHandler.IsPlayerBuilding)
        {

            var food = ResourceManager.Instance.FindResource(ResourceType.food);
            int foodCost = spawnBuildingHandler.UnitData.UnitFoodCost;
            if (food.Amount >= foodCost)
            {
                food.DecreaseAmount(foodCost);
                SpawnUnits(spawnBuildingHandler);
            }
        }
        else
        {
            SpawnUnits(spawnBuildingHandler);
        }
    }
    private void SpawnUnits(SpawnBuildingHandler spawnBuildingHandler)
    {
        UnitData unitData = spawnBuildingHandler.UnitData;

        Vector3 buildingPos = spawnBuildingHandler.transform.position;
        Vector3 spawnPoint = new Vector3(buildingPos.x + Random.Range(-10, 10), buildingPos.y + Random.Range(-10, 10), 0);
        if (spawnBuildingHandler.IsPlayerBuilding)
        {

            Transform parentTransform = PlayerUnitsManagers.Instance.transform;
            GameObject unitsManager = Instantiate(_playerUnitsManagerPrefab, parentTransform);  

            UnitsManager manager = unitsManager.GetComponent<UnitsManager>();
            manager.Initialize(unitData, spawnPoint, true);

            TurnManager.Instance.PlayerUnitsManagers.Add(manager);
        }
        else
        {
                Transform parentTransform = spawnBuildingHandler.transform;
                GameObject unitsManager = Instantiate(_enemieUnitsManagerPrefab, parentTransform);
    
                EnemieUnitsManager manager = unitsManager.GetComponent<EnemieUnitsManager>();
                manager.Initialize(unitData, spawnPoint, false);
    
                TurnManager.Instance.EnemieUnitsManagers.Add(manager);
        }
    }
}
