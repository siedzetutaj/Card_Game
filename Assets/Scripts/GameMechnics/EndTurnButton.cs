using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    public void OnEndTurnButtonPressed()
    {
        TurnManager.Instance.EndTacticalPhaseRequest();
    }
}
