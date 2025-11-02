using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BuildingCardEffect", menuName = "Scriptable Objects/Effects/BuildingCardEffect")]
public class BuildingCardEffectSO : CardEffectSO
{
    public BuildingSO BuildingSO;
    public GameObject BuildingPrefab;
    [NonSerialized] public CardType CardType = CardType.Building;
    public override void ApplyEffect(GameObject groundTile)
    {
        GameObject building = Instantiate(BuildingPrefab, groundTile.transform);
        building.GetComponent<BuildingHandler>().Initialize(BuildingSO);  
    }
}
