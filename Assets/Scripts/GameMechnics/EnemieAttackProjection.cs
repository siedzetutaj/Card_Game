using System.Collections.Generic;
using UnityEngine;

public class EnemieAttackProjection : MonoBehaviourSingleton<EnemieAttackProjection>
{
    [SerializeField] private GameObject EnemieAttackInfoPrefab;
    private List<GameObject> AttackInfoGameObjects = new();
   
    public void ClearAttackProjections()
    {
        foreach (GameObject attackInfoGO in AttackInfoGameObjects)
            Destroy(attackInfoGO);
        AttackInfoGameObjects.Clear();
    }

    public void SetAttackProjection(Sprite sprite, int amount)
    {
        EnemieAttackInfo enemieAttackInfo = CreateEnemieAttackInfo();
        enemieAttackInfo.EnemieImage.sprite = sprite;
        enemieAttackInfo.EnemieAmount.text = amount.ToString();
    }

    public EnemieAttackInfo CreateEnemieAttackInfo()
    {
        GameObject enemieAttackInfoGO = Instantiate(EnemieAttackInfoPrefab, transform);
        AttackInfoGameObjects.Add(enemieAttackInfoGO);
        EnemieAttackInfo enemieAttackInfo = enemieAttackInfoGO.GetComponent<EnemieAttackInfo>();
        return enemieAttackInfo;
    }
}

