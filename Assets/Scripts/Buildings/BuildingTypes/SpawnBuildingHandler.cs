using UnityEngine;

public class SpawnBuildingHandler : BuildingHandler
{
    private UnitData _unitData;
    public UnitData UnitData
    {
        get => _unitData;
        private set => _unitData = value;
    }
    public override void Initialize(BuildingSO buildingSO)
    {
        base.Initialize(buildingSO);

        SpawnBuildingSO spawnBuildingSO = buildingSO as SpawnBuildingSO;
        _unitData = new(spawnBuildingSO.UnitSO.UnitData);
    }
}
