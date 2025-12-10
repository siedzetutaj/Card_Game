using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "Scriptable Objects//Builidings/BuildingSO")]
public class BuildingSO : ScriptableObject
{
    public Sprite Sprite;
    public UnitSO UnitSO;
    public List<BuildingOnEndTurnEffectSO> OnEndTurnEffects = new();

}
