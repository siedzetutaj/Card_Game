using UnityEngine;

public class ShootBuildingHandler : BuildingHandler
{
    public int _damage;
    public int _range;
    public float _attackSpeed;

    public override void Initialize(BuildingSO buildingSO)
    {
        base.Initialize(buildingSO);
        
        ShootBuildingSO shootBuildingSO = buildingSO as ShootBuildingSO;
        _damage = shootBuildingSO.Damage;
        _range = shootBuildingSO.Range;
        _attackSpeed = shootBuildingSO.AttackSpeed;
    }
}
