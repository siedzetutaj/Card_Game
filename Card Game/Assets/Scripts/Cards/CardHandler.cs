using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : InteractableObject
{
    public CardType CardType;

    private ResourceManager _resourceManager;

    [SerializeField] private CardSO _cardSO;

    [SerializeField] private GameObject _highlight;
    [SerializeField] private TextMeshProUGUI _cardNameTmp;
    [SerializeField] private TextMeshProUGUI _cardDescriptionTmp;
    [SerializeField] private Image _cardImage;
    protected override void OnEnable()
    {
        base.OnEnable();
        _resourceManager = ResourceManager.Instance;
        _highlight.SetActive(false);

        _cardNameTmp.text = _cardSO.Name;
        _cardDescriptionTmp.text = _cardSO.Description;
        _cardImage.sprite = _cardSO.Sprite;
        CardType = _cardSO.Type;
    }
    protected override void OnObjectClicked()
    {
        bool isEnoughtResources = true;

        foreach (var resourceTypeCost in _cardSO.Cost)
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
    public virtual void OnCardPlayed(GameObject groundTile)
    {

        foreach (var resourceTypeCost in _cardSO.Cost)
        {
            _resourceManager.FindResource(resourceTypeCost.Type).Amount -= resourceTypeCost.Cost;
        }
        _cardSO.OnCardPlayed(groundTile);
        Destroy(gameObject);
    }
    public virtual void DisableHighlight()
    {
        _highlight.SetActive(false);
    }
}
