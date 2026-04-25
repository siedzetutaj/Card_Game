using UnityEngine;

[CreateAssetMenu(fileName = "SpawnBuildingSO", menuName = "Scriptable Objects/Buildings/Type/Spawn")]
public class SpawnBuildingSO : BuildingSO
{
    [Header("Spawn")]
    public UnitSO UnitSO;
}
