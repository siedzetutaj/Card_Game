using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardSO", menuName = "Scriptable Objects/CardSO")]
public class CardSO : ScriptableObject
{
    public string Name;
    public CardType Type;
    public List<ResourceTypeCost> Cost = new();
    [TextArea(5, 20)]
    public string Description;
    public Sprite Sprite;
    //W edytorze dorobiæ wyszukiwarkê efektów po typie karty
    public List<CardEffectSO> Effects = new();
    public virtual void OnCardPlayed(GameObject interactiveObject)
    {
        foreach (var effect in Effects)
        {
            effect.ApplyEffect(interactiveObject);
        }
    }
}
