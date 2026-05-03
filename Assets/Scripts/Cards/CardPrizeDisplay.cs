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
    public void SetCard(CardSO card)
    {
        CardData = new CardData(card);

        _cardNameTmp.text = CardData.Name;
        _cardDescriptionTmp.text = CardData.Description;
        _cardImage.sprite = CardData.Sprite;
    }
    protected override void OnObjectClicked()
    {
        Debug.Log("Card Prize Clicked, map should open");
        DeckManager.Instance.AddCardToDeck(CardData);
        RewardsManager.Instance.InvokeOnRewardChosen();
    }

}
