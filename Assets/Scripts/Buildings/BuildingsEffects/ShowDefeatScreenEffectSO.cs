using UnityEngine;

[CreateAssetMenu(fileName = "ShowDefeatScreen", menuName = "Scriptable Objects/Buildings/Effects/ShowDefeatScreen")]
public class ShowDefeatScreenEffectSO : BuildingEffectSO
{
    public override void ApplyBuildingEffect(BuildingHandler buildingHandler)
    {
        LoseScreen.Instance.ShowLoseScreen();
    }
}