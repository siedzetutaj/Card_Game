using UnityEngine;

public class LoseScreen : MonoBehaviourSingleton<LoseScreen>
{
    [SerializeField] private GameObject _loseScreen;

    public void ShowLoseScreen()
    {
        Time.timeScale = 0f;
        _loseScreen.SetActive(true);
    }
}
