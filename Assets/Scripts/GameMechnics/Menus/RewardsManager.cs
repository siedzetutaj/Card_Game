using System.Collections.Generic;
using UnityEngine;

public class RewardsManager : MonoBehaviourSingleton<RewardsManager>
{
    [Header("Main Info")]
    [SerializeField] private GameObject _rewardsScreen;

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
        ToggleRewardsUI(false);
    }

    private void ToggleRewardsUI(bool isOn)
    {
        _rewardsScreen.SetActive(isOn);
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
        ToggleRewardsUI(true);
        ResetCardRewards();
        GenerateCardRewards(3);
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
            newCardDisplay.SetCard(randomCard);
            _currentCardRewardList.Add(newCardDisplay);
            
            if (tempCardList.Count > 1)
                tempCardList.RemoveAt(randomIndex);
        }   
    }

    public void InvokeOnRewardChosen()
    {
        _onRewardChosen?.Invoke();
        ToggleRewardsUI(false);
    }
}
