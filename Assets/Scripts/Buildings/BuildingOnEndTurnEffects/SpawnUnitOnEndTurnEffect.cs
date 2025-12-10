using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SpawnUnit", menuName = "Scriptable Objects/Buildings/OnEndTurn/SpawnUnit")]
public class SpawnUnitOnEndTurnEffectSO : BuildingOnEndTurnEffectSO
{
    [SerializeField] private GameObject _unitsManagerPrefab;

    public override void ApplyOnEndTurnEffect(BuildingHandler buildingHandler)
    {
        SpawnBuildingHandler spawnBuildingHandler = buildingHandler as SpawnBuildingHandler;

        var food = ResourceManager.Instance.FindResource(ResourceType.food);
        int foodCost = spawnBuildingHandler.UnitData.UnitFoodCost;
        if (food.Amount >= foodCost)
        {
            food.SetAmount(food.GetAmount() - foodCost);
            SpawnUnits(spawnBuildingHandler);
        }
    }
    private void SpawnUnits(SpawnBuildingHandler spawnBuildingHandler)
    {
        UnitData unitData = spawnBuildingHandler.UnitData;

        Vector3 buildingPos = spawnBuildingHandler.transform.position;
        Vector3 spawnPoint = new Vector3(buildingPos.x, buildingPos.y, 0);

        Transform parentTransform = PlayerUnitsManagers.Instance.transform;
        GameObject unitsManager = Instantiate(_unitsManagerPrefab, parentTransform);
        
        unitsManager.GetComponent<UnitsManager>().Initialize(unitData, spawnPoint, true);

        GameLogicManager.Instance.PlayerUnitsManagers.Add(unitsManager.GetComponent<UnitsManager>());
    }
}
