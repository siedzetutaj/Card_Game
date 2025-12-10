using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviourSingleton<DeckManager>
{

    private ResourceManager _resourceManager => ResourceManager.Instance;
    private HandController _handController => HandController.Instance;
    private GameLogicManager _gameLogicManager => GameLogicManager.Instance;


    [SerializeField] private List<CardData> _cardsInDeck = new();
    [SerializeField] private List<CardData> _cardsInDrawPile = new();
    [SerializeField] private List<CardData> _cardsInDiscardPile = new();
    [SerializeField] private List<CardHandler> _cardsInHand = new();
 

    [SerializeField] private GameObject CardPrefab; 
    [SerializeField] private List<CardSO> StartingDeck;

    private void OnEnable()
    {
        foreach (var cardSO in StartingDeck)
        {
            _cardsInDeck.Add(new CardData(cardSO));
        }
    }
    public void OnFirstTurn(int moneyAmount)
    {
        _resourceManager.FindResource(ResourceType.money)?.SetAmount(moneyAmount);
        _cardsInDrawPile = new List<CardData>(_cardsInDeck);
        _cardsInDiscardPile.Clear();
        _cardsInHand.Clear();
        DrawCards();
    }
    public void NextTurn(int moneyAmount)
    {
        _resourceManager.FindResource(ResourceType.money)?.SetAmount(moneyAmount);

        for (int i = _cardsInHand.Count - 1; i >= 0; i--)
        {
            DiscardCard(_cardsInHand[i]);
        }
        DrawCards();
    }
    public void AddCardToDeck(CardData cardData)
    {
        if (_cardsInDeck.Contains(cardData))
        {
            Debug.Log("Card Already in deck");
            return;
        }
        _cardsInDeck.Add(cardData);

    }
    public void RemoveCardFromDeck(CardData cardData)
    {
        if (!_cardsInDeck.Contains(cardData))
        {
            Debug.Log("Card does not exists in deck");
            return;
        }
        _cardsInDeck.Remove(cardData);
    }
    public void DrawCards(int amount = 3)
    {
        for(int i = 0; i < amount; i++)
        {
            if (_cardsInDrawPile.Count == 0)
            {
                _cardsInDrawPile = new List<CardData>(_cardsInDiscardPile);
                _cardsInDiscardPile.Clear();
            }
            if (_cardsInDrawPile.Count == 0)
            { 
                Debug.Log("No Cards");
                break; 
            }
            int randomIndex = Random.Range(0, _cardsInDrawPile.Count);
            CardData drawnCard = _cardsInDrawPile[randomIndex];
            _cardsInDrawPile.RemoveAt(randomIndex);
            AddCardToHand(drawnCard);
        }
    }
    private void AddCardToHand(CardData drawnCard)
    {

        GameObject cardInHand = Instantiate(CardPrefab);
        CardHandler handler = cardInHand.GetComponent<CardHandler>();
        handler.Initialize(drawnCard);

        _cardsInHand.Add(handler);
        _handController.AddCard(handler);
    }
    public void DiscardCard(CardHandler cardHandler)
    {
        if (_cardsInHand.Contains(cardHandler))
        {
            _cardsInHand.Remove(cardHandler);
            _cardsInDiscardPile.Add(cardHandler.CardData);
            _handController.RemoveCard(cardHandler);
            cardHandler.DestroyCard();
        }
    }  
    public void DiscardAllCardsInHand()
    {
        while (_cardsInHand.Count > 0)
        {
            DiscardCard(_cardsInHand[0]);
        }
    }
    public void DiscardCardsInHand(int number)
    {
        if(_cardsInHand.Count == 0)
            return;
        
        for(int i = 0; i < number; i++)
        {
            int randomIndex = Random.Range(0, _cardsInHand.Count);
            CardHandler CardToDiscard = _cardsInHand[randomIndex];
            DiscardCard(CardToDiscard);
        }

    }
    public bool IsHandEmpty()
    {
        return _cardsInHand.Count == 0;
    }
}
