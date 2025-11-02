using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData 
{
    public string Name;
    public CardType Type;
    public List<ResourceTypeCost> Cost = new();
    [TextArea(5, 20)]
    public string Description;
    public Sprite Sprite;
    public readonly string CardID;
    //W edytorze dorobiæ wyszukiwarkê efektów po typie karty
    public List<CardEffectSO> Effects = new();
    
    public CardData(CardData cardData)
    {
        Name = cardData.Name;
        Type = cardData.Type;
        Cost = new List<ResourceTypeCost>(cardData.Cost);
        Description = cardData.Description;
        Sprite = cardData.Sprite;
        CardID = cardData.CardID;
        Effects = new List<CardEffectSO>(cardData.Effects);
    }
    public CardData(CardSO cardSO)
    {
        Name = cardSO.Name;
        Type = cardSO.Type;
        Cost = new List<ResourceTypeCost>(cardSO.Cost);
        Description = cardSO.Description;
        Sprite = cardSO.Sprite;
        CardID = System.Guid.NewGuid().ToString();
        Effects = new List<CardEffectSO>(cardSO.Effects);
    }

    public virtual void OnCardPlayed(GameObject interactiveObject)
    {
        foreach (var effect in Effects)
        {
            effect.ApplyEffect(interactiveObject);
        }
    }
}
