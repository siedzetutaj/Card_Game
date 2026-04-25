using System.Collections.Generic;
using UnityEngine;

public class VictoryScreen : MonoBehaviourSingleton<VictoryScreen>
{
    /*Wyœwietlenie kart 
     * Trzeba zrobiæ pule kart, z której bêdzie sie losowaæ
     * 
     */
    [SerializeField] private GameObject _victoryScreen;
    [SerializeField] private GameObject _cardPrizePanel;
    [SerializeField] private GameObject _cardPrizePrefab;
    [SerializeField] private List<CardSO> CardPool = new List<CardSO>();
    public AdditionalCardsPoolSO AdditionalCardsPoolSO;
    private void Start()
    {
        foreach (CardSO cardSO in AdditionalCardsPoolSO.AdditionalCardsPool)
        {
            CardPool.Add(cardSO);
        }
    }
    [ContextMenu("Show Victory Screen")]
    public void ShowVictoryScreen()
    {
        Time.timeScale = 0f;
        _victoryScreen.SetActive(true);
        GenerateCards();
    }

    private void GenerateCards()
    {
        for(int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, CardPool.Count);
            CardSO randomCard = CardPool[randomIndex];
            GameObject cardGO = Instantiate(_cardPrizePrefab, _cardPrizePanel.transform);
            CardPrizeDisplay cardDisplay = cardGO.GetComponent<CardPrizeDisplay>();
            cardDisplay.SetCard(randomCard);
        }   
    }
}
