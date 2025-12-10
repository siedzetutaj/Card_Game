using UnityEngine;

[CreateAssetMenu(fileName = "ShootBuilding", menuName = "Scriptable Objects/Buildings/OnEndFight/ShootBuilding")]
public class ShootOnEndFightEffect : BuildingOnEndFightEffectSO
{
    //TODO:
    //Prefab pocisku
    //Targetowanie przeciwnikow

    public override void ApplyOnEndFightEffect(BuildingHandler buildingHandler)
    {
        ShootBuildingHandler shootBuildingHandler = buildingHandler as ShootBuildingHandler;
    }
}
