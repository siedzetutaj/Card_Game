using UnityEngine;

[CreateAssetMenu(fileName = "ShootBuildingSO", menuName = "Scriptable Objects/Buildings/Type/Shoot")]
public class ShootBuildingSO : BuildingSO
{
    [Header("Shoot")]
    public int Damage;
    public int Range;
    public float AttackSpeed;
}
