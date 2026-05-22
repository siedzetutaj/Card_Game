using UnityEngine;

[CreateAssetMenu(fileName = "ShootBuilding", menuName = "Scriptable Objects/Buildings/Effects/ShootBuilding")]
public class ShootBuildingEffectSO : BuildingEffectSO
{
    [SerializeField] private int _damage;
    [SerializeField] private int _range;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private GameObject _rangeMarkerPrefab;

    public override void ApplyBuildingEffect(BuildingHandler buildingHandler)
    {
        // Spawn RangeMarker as a child of the building if prefab is assigned
        if (_rangeMarkerPrefab != null && buildingHandler.GetComponentInChildren<RangeMarker>(true) == null)
        {
            var marker = Instantiate(_rangeMarkerPrefab, buildingHandler.transform);
            var rangeMarker = marker.GetComponent<RangeMarker>();
            if (rangeMarker != null)
            {
                rangeMarker.DisableVisiblity();
            }
        }

        BuildingShooter shooter = buildingHandler.GetComponent<BuildingShooter>();
        if (shooter == null)
        {
            shooter = buildingHandler.gameObject.AddComponent<BuildingShooter>();
        }
        shooter.Setup(_damage, _range, _attackSpeed, buildingHandler);
    }
}
