using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseBuildingSO", menuName = "Scriptable Objects/Buildings/Type/Base")]
public class BuildingSO : ScriptableObject
{
    [Header("Base")]
    public Sprite Sprite;
    public int health;
    [Header("Beginning Effects")]
    public List<BuildingEffectSO> OnBeginningBuildingEffects = new();

    [Header("Tactical Effects")]
    public List<BuildingEffectSO> OnBeginningTacticalEffects = new();
    public List<BuildingEffectSO> OnEndTacticalEffects = new();
    [Header("Combat Effects")]
    public List<BuildingEffectSO> OnBeginningCombatEffects = new();
    public List<BuildingEffectSO> OnEndCombatEffects = new();

    //test czy git dziala
}
