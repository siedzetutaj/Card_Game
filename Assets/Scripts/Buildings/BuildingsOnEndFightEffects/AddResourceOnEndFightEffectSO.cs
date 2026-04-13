using UnityEngine;

[CreateAssetMenu(fileName = "AddResource", menuName = "Scriptable Objects/Buildings/OnEndFight/AddResource")]
public class AddResourceOnEndFightEffectSO : BuildingOnEndFightEffectSO
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private int _amount;

    public override void ApplyOnEndFightEffect(BuildingHandler buildingHandler)
    {
        ResourceHandler resource;

        if (buildingHandler.IsPlayerBuilding)
            resource = ResourceManager.Instance.FindResource(_resourceType);
        else
            resource = EnemieResourceManager.Instance.FindResource(_resourceType);

        resource?.IncreaseAmount(_amount);
    }
}
