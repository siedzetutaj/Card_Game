using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SpawnUnit", menuName = "Scriptable Objects/BuildingOnEndTurn/SpawnUnit")]
public class SpawnUnitOnEndTurnEffectSO : BuildingOnEndTurnEffectSO
{
    [SerializeField] private GameObject _unitsManagerPrefab;

    public override void ApplyOnEndTurnEffect(BuildingHandler buildingHandler)
    {
        var food = ResourceManager.Instance.FindResource(ResourceType.food);
        food.SetAmount(food.GetAmount() - buildingHandler.UnitData.UnitFoodCost);
        SpawnUnits(buildingHandler);
    }
    private void SpawnUnits(BuildingHandler buildingHandler)
    {
        UnitData unitData = buildingHandler.UnitData;

        Transform spawnPoint = buildingHandler.transform;

        GameObject unitsManager = Instantiate(_unitsManagerPrefab, spawnPoint);
        unitsManager.GetComponent<UnitsManager>().Initialize(unitData, spawnPoint, true);

        GameLogicManager.Instance.PlayerUnitsManagers.Add(unitsManager.GetComponent<UnitsManager>());
    }
}
