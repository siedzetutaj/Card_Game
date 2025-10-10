using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : InteractableObject
{
    public CardType CardType;
    private ResourceManager _resourceManager;

    public CardData CardData;

    [SerializeField] private GameObject _highlight;
    [SerializeField] private TextMeshProUGUI _cardNameTmp;
    [SerializeField] private TextMeshProUGUI _cardDescriptionTmp;
    [SerializeField] private Image _cardImage;
    
    public void Initialize(CardData cardData)
    {
        CardData = cardData;
        _resourceManager = ResourceManager.Instance;
        _highlight.SetActive(false);

        _cardNameTmp.text = CardData.Name;
        _cardDescriptionTmp.text = CardData.Description;
        _cardImage.sprite = CardData.Sprite;
        CardType = CardData.Type;
    }
    protected override void OnObjectClicked()
    {
        bool isEnoughtResources = true;

        foreach (var resourceTypeCost in CardData.Cost)
        {
            if (!_resourceManager.FindResource(resourceTypeCost.Type).IsEnoughtResources(resourceTypeCost.Cost))
            {
                isEnoughtResources = false;
                return;
            }
        }
        if (isEnoughtResources)
        {
            SelectedCard.Instance.Card = this;  
            _highlight.SetActive(true);
        }
    }
    protected override void OnObjectMouseEnter()
    {
    }
    protected override void OnObjectMouseExit()
    {
    }
    public virtual void OnCardPlayed(GameObject interactiveObject)
    {

        foreach (var resourceTypeCost in CardData.Cost)
        {
            _resourceManager.FindResource(resourceTypeCost.Type).Amount -= resourceTypeCost.Cost;
        }
        CardData.OnCardPlayed(interactiveObject);

        DeckManager.Instance.DiscardCard(this);
    }
    public virtual void DestroyCard()
    {
        Destroy(gameObject);
    }
    public virtual void DisableHighlight()
    {
        _highlight.SetActive(false);
    }
}
