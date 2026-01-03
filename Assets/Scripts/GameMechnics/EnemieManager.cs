using TMPro;
using UnityEngine;

public class EnemieManager : MonoBehaviourSingleton<EnemieManager>
{
    [SerializeField] private TextMeshProUGUI _healthPointsText;
    private int _healthPoints;
    public int HealthPoints
    {
        get => _healthPoints;
        set
        {
            _healthPointsText.text = value.ToString();
            _healthPoints = value;
        }
    }
}
