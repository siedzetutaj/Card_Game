using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    public void OnEndTurnButtonPressed()
    {
        GameLogicManager.Instance.EndTurn();
    }
}
