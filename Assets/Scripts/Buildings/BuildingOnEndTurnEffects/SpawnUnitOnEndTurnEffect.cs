using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SpawnUnit", menuName = "Scriptable Objects/BuildingOnEndTurn/SpawnUnit")]
public class SpawnUnitOnEndTurnEffectSO : BuildingOnEndTurnEffectSO
{
    [SerializeField] private GameObject _unitsManagerPrefab;

    public override void ApplyOnEndTurnEffect(BuildingHandler buildingHandler)
    {
        var food = ResourceManager.Instance.FindResource(ResourceType.food);
        int foodCost = buildingHandler.UnitData.UnitFoodCost;
        if (food.Amount > foodCost)
        {
            food.SetAmount(food.GetAmount() - foodCost);
            SpawnUnits(buildingHandler);
        }
    }
    private void SpawnUnits(BuildingHandler buildingHandler)
    {
        UnitData unitData = buildingHandler.UnitData;

        Vector3 buildingPos = buildingHandler.transform.position;
        Vector3 spawnPoint = new Vector3(buildingPos.x, buildingPos.y, 0);

        Transform parentTransform = PlayerUnitsManagers.Instance.transform;
        GameObject unitsManager = Instantiate(_unitsManagerPrefab, parentTransform);
        
        unitsManager.GetComponent<UnitsManager>().Initialize(unitData, spawnPoint, true);

        GameLogicManager.Instance.PlayerUnitsManagers.Add(unitsManager.GetComponent<UnitsManager>());
    }
}
