using UnityEngine;

[CreateAssetMenu(fileName = "ShootBuilding", menuName = "Scriptable Objects/Buildings/Effects/ShootBuilding")]
public class ShootBuildingEffectSO : BuildingEffectSO
{
    [SerializeField] private int _damage;
    [SerializeField] private int _range;
    [SerializeField] private float _attackSpeed;

    public override void ApplyBuildingEffect(BuildingHandler buildingHandler)
    {
        BuildingShooter shooter = buildingHandler.GetComponent<BuildingShooter>();
        if (shooter == null)
        {
            shooter = buildingHandler.gameObject.AddComponent<BuildingShooter>();
        }
        shooter.Setup(_damage, _range, _attackSpeed, buildingHandler);
    }
}
