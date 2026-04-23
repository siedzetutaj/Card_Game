using UnityEngine;

[CreateAssetMenu(fileName = "ShootBuilding", menuName = "Scriptable Objects/Buildings/Effects/ShootBuilding")]
public class ShootBuildingEffectSO : BuildingEffectSO
{
    //TODO:
    //Prefab pocisku
    //Targetowanie przeciwnikow

    public override void ApplyBuildingEffect(BuildingHandler buildingHandler)
    {
        ShootBuildingHandler shootBuildingHandler = buildingHandler as ShootBuildingHandler;
    }
}
