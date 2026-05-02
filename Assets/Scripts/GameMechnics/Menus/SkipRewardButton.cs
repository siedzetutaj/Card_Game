using UnityEngine;

public class SkipRewardButton : MonoBehaviour
{
    public void SkipReward()
    {
        //Po klikni�ciu powinno przenie�� gracza do mapy z wyborem kolejnego spotaknia
        Debug.Log("Reward skipped!");
        MapController.Instance.ChangeState(MapState.ChoosingEvent);
    }
}
