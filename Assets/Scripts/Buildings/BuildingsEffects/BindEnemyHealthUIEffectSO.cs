using UnityEngine;

[CreateAssetMenu(fileName = "BindEnemyHealthUI", menuName = "Scriptable Objects/Buildings/Effects/BindEnemyHealthUI")]
public class BindEnemyHealthUIEffectSO : BuildingEffectSO
{
    public override void ApplyBuildingEffect(BuildingHandler buildingHandler)
    {
        EnemieManager.Instance.BindToBase(buildingHandler);
    }
}