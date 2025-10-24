using TMPro;
using UnityEngine;

public class EnemieManager : MonoBehaviourSingleton<EnemieManager>
{
    [SerializeField] private TextMeshProUGUI _healthPointsText;
    private int _healthPoints = 3;
    public int HealthPoints
    {
        get => _healthPoints;
        set
        {
            _healthPointsText.text = value.ToString();
            _healthPoints = value;
            if (_healthPoints <= 0)
            {
                GameLogicManager.Instance.EnemieDefeated();
            }
        }
    }
    private void Start()
    {
        _healthPointsText.text = _healthPoints.ToString();
    }
}
