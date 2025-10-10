using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DeckManager : MonoBehaviourSingleton<DeckManager>
{

    private int _turn = 0;
    [SerializeField] private List<CardData> _cardsInDeck = new();
    [SerializeField] private List<CardData> _cardsInDrawPile = new();
    [SerializeField] private List<CardData> _cardsInDiscardPile = new();
    [SerializeField] private List<CardHandler> _cardsInHand = new();
 
    private HandController _handController;

    [SerializeField] private GameObject CardPrefab; 
    [SerializeField] private List<CardSO> StartingDeck;

    private void OnEnable()
    {
        foreach (var cardSO in StartingDeck)
        {
            _cardsInDeck.Add(new CardData(cardSO));
        }

        _handController = HandController.Instance;
    }
    public void OnFirstTurn()
    {
        if(_turn == 0)
        {
            _cardsInDrawPile = new List<CardData>(_cardsInDeck);
            _cardsInDiscardPile.Clear();
            _cardsInHand.Clear();
            DrawCards();
        }
        else 
        {
            NextTurn();
        }
            _turn++;
    }
    public void NextTurn()
    {
        for(int i = _cardsInHand.Count - 1; i >= 0; i--)
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
            if (_cardsInDrawPile.Count == 0) Debug.Log("No Cards");

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
}
