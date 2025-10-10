using UnityEngine;

[CreateAssetMenu(fileName = "RecruitmentCardEffectSO", menuName = "Scriptable Objects/Effects/RecruitmentCardEffectSO")]
public class RecruitmentCardEffectSO : CardEffectSO
{
    [SerializeField] private GameObject _unitsManagerPrefab;
    public override void ApplyEffect(GameObject building)
    {
        UnitData unitData = building.GetComponent<BuildingHandler>().UnitData;

        Transform spawnPoint = UnitsSpawnPoints.Instance.FindSpawnPoint(unitData.SpawnPosionIndex);

        GameObject unitsManager = Instantiate(_unitsManagerPrefab, spawnPoint);
        unitsManager.GetComponent<UnitsManager>().Initialize(unitData, spawnPoint, true);
        GameLogicManager.Instance.PlayerUnits = unitsManager.GetComponent<UnitsManager>();
    }
}
