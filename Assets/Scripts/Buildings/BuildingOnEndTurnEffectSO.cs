using UnityEngine;

public abstract class BuildingOnEndTurnEffectSO : ScriptableObject
{
    public abstract void ApplyOnEndTurnEffect(BuildingHandler buildingHandler);
}
