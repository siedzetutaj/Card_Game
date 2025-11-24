using UnityEngine;

[CreateAssetMenu(fileName = "SpawnUnit", menuName = "Scriptable Objects/BuildingOnEndTurn/SpawnUnit")]
public class SpawnUnitOnEndTurnEffect : BuildingOnEndTurnEffect
{
    public UnitSO UnitSO;
    public int Amount;
    public int FoodCost;    
    public override void ApplyOnEndTurnEffect()
    {
        var food = ResourceManager.Instance.FindResource(ResourceType.food);
        food.SetAmount(food.GetAmount() - FoodCost);
        //Spawn units
    }
}
