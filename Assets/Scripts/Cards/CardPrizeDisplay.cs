using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPrizeDisplay : InteractableObject
{
    public CardData CardData;

    [SerializeField] private TextMeshProUGUI _cardNameTmp;
    [SerializeField] private TextMeshProUGUI _cardDescriptionTmp;
    [SerializeField] private Image _cardImage;

    [Header("Shop")]
    [SerializeField] private int _cost = 0;
    [SerializeField] private TextMeshProUGUI _costTMP;
    public void SetCard(CardSO card, int newCost = 0)
    {
        CardData = new CardData(card);

        _cardNameTmp.text = CardData.Name;
        _cardDescriptionTmp.text = CardData.Description;
        _cardImage.sprite = CardData.Sprite;

        _cost = Mathf.Clamp(newCost, 0, 100000000); //duzo xd

        if (_cost > 0)
            _costTMP.text = $"Cost: {_cost}";
        else
            _costTMP.text = "";
    }
    protected override void OnObjectClicked()
    {
        if (ResourceManager.Instance.FindResource(ResourceType.money).Amount < _cost)
            return;

        Debug.Log("Card Prize Clicked");
        DeckManager.Instance.AddCardToDeck(CardData);

        if (_cost == 0) //troche slaby fix ale na razie przezyjemy
            RewardsManager.Instance.InvokeOnRewardChosen();
        else
        {
            //dla sklepu
            ResourceManager.Instance.FindResource(ResourceType.money).Amount -= _cost;
            Destroy(gameObject);
        }
    }

}
