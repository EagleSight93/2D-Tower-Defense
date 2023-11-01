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

    [SerializeField] Hand hand;
    
    void Start()
    {
        DrawHand();
    }

    [ContextMenu("Draw a Card")]
    public void DrawCard()
    {
        int index = Random.Range(0, cardDatas.Length);
        Card newCard = Instantiate(cardPrefab, hand.transform, false);
        newCard.data = cardDatas[index];
        newCard.RenderData();
        hand.AddCard(newCard);
    }

    [ContextMenu("Draw hand")]
    public void DrawHand()
    {
        for (int i = 0; i < hand.handSize; i++)
            DrawCard();
    }
}