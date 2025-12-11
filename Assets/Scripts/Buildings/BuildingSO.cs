using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseBuildingSO", menuName = "Scriptable Objects/Buildings/Type/Base")]
public class BuildingSO : ScriptableObject
{
    [Header("Base")]
    public Sprite Sprite;
    public List<BuildingOnEndTurnEffectSO> OnEndTurnEffects = new();
    public List<BuildingOnEndFightEffectSO> OnEndFightEffects = new();
}
