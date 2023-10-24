using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    public CardData[] cardDatas;
    public Card cardPrefab;
    public int handSize;
    public int rewardSize;

    [SerializeField] Hand handPrefab;
    Hand _hand;

    void Start()
    {
        InitHand();
    }

    [ContextMenu("Draw a Card")]
    public void DrawCard()
    {
        int index = Random.Range(0, cardDatas.Length);
        Card newCard = Instantiate(cardPrefab, _hand.transform, false);
        newCard.data = cardDatas[index];
        newCard.RenderData();

        _hand.AddCard(newCard);
    }

    [ContextMenu("Draw hand")]
    public void DrawHand()
    {
        for (int i = 0; i < handSize; i++)
            DrawCard();
    }

    void InitHand()
    {
        _hand = Instantiate(handPrefab, transform, false);
        _hand.deck = this;
        _hand.name = "Hand";
    }
}