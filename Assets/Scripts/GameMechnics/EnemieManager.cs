using TMPro;
using UnityEngine;

public class EnemieManager : MonoBehaviourSingleton<EnemieManager>
{
    [SerializeField] private TextMeshProUGUI _healthPointsText;
    private EnemieAI _enemieAI;

    public void SetEnemyAI(EnemieAI enemieAI)
    {
        _enemieAI = enemieAI;
    }

    public void BindToBase(BuildingHandler baseHandler)
    {
        _healthPointsText.text = baseHandler.Health.ToString();
        if (_enemieAI != null)
            _enemieAI.CurrentHealth = baseHandler.Health;

        baseHandler.OnHealthChanged += UpdateHealthDisplay;
    }

    private void UpdateHealthDisplay(int currentHealth)
    {
        _healthPointsText.text = currentHealth.ToString();
        if (_enemieAI != null)
            _enemieAI.CurrentHealth = currentHealth;
    }
}