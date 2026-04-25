using TMPro;
using UnityEngine;

public class EnemieManager : MonoBehaviourSingleton<EnemieManager>
{
    [SerializeField] private TextMeshProUGUI _healthPointsText;
    private EnemieAI _enemieAI;
    private int _healthPoints;
    public int HealthPoints
    {
        get => _healthPoints;
        set
        {
            _healthPointsText.text = value.ToString();
            _healthPoints = value;
            _enemieAI.CurrentHealth = value;
            if (_healthPoints <= 0)
            {
                Debug.Log("Enemie Defeated! Player Wins!");
                VictoryScreen.Instance.ShowVictoryScreen();
            }
        }
    }
    public void SetEnemyAI(EnemieAI enemieAI)
    {
        _enemieAI = enemieAI;
    }   
}
