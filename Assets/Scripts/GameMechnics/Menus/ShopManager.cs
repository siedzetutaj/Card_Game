using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviourSingleton<ShopManager>
{
    [Header("Main Info")]
    [SerializeField] private GameObject _shopScreen;

    [Header("Card Rewards")]
    [SerializeField] private GameObject _cardsScreen;
    [SerializeField] private GameObject _cardPrizePanel;
    [SerializeField] private CardPrizeDisplay _cardPrizePrefab;
    [SerializeField] private List<CardPrizeDisplay> _currentCardRewardList = new();
    [SerializeField] private List<CardSO> _cardPool = new();

    public AdditionalCardsPoolSO AdditionalCardsPoolSO;

    //Other
    private System.Action _onRewardChosen;

    private void Start()
    {
        UpdateRewardsManager(AdditionalCardsPoolSO);
        ToggleUI(false);
    }

    private void ToggleUI(bool isOn)
    {
        _cardsScreen.SetActive(isOn);
    }

    //w przyszlosci pewnie zalezne od wybranego gatunku
    public void UpdateRewardsManager(AdditionalCardsPoolSO newCardPool)
    {
        _cardPool.Clear();
        foreach (CardSO cardSO in newCardPool.AdditionalCardsPool)
            _cardPool.Add(cardSO);
    }

    public void ShowCardRewards(System.Action newOnRewardChosen)
    {
        _onRewardChosen = newOnRewardChosen;
        ToggleUI(true);
        ResetCardRewards();
        GenerateCardRewards(8);
    }

    private void ResetCardRewards()
    {
        for (int i = 0; i < _currentCardRewardList.Count; i++)
            if (_currentCardRewardList[i] != null)
                Destroy(_currentCardRewardList[i].gameObject);

        _currentCardRewardList.Clear();
    }

    private void GenerateCardRewards(int howManyCards)
    {
        List<CardSO> tempCardList = new List<CardSO>(_cardPool);
        
        for(int i = 0; i < howManyCards; i++)
        {
            int randomIndex = Random.Range(0, tempCardList.Count);
            CardSO randomCard = tempCardList[randomIndex];
            CardPrizeDisplay newCardDisplay = Instantiate(_cardPrizePrefab, _cardPrizePanel.transform);
            newCardDisplay.SetCard(randomCard, Random.Range(4,20));
            _currentCardRewardList.Add(newCardDisplay);
            
            if (tempCardList.Count > 1)
                tempCardList.RemoveAt(randomIndex);
        }   
    }

    public void InvokeOnRewardChosen()
    {
        _onRewardChosen?.Invoke();
        //ToggleUI(false);
    }

    public void DeactivateShop()
    {
        ToggleUI(false);
        MapController.Instance.ChangeState(MapState.ChoosingEvent);
    }
}
