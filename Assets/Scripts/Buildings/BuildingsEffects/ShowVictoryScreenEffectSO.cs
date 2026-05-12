using UnityEngine;

[CreateAssetMenu(fileName = "ShowVictoryScreen", menuName = "Scriptable Objects/Buildings/Effects/ShowVictoryScreen")]
public class ShowVictoryScreenEffectSO : BuildingEffectSO
{
    public override void ApplyBuildingEffect(BuildingHandler buildingHandler)
    {
        TurnManager.Instance.OnVictory();
        RewardsManager.Instance.ShowCardRewards(
            () => MapController.Instance.ChangeState(MapState.ChoosingEvent));
    }
}