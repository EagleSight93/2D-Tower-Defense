using System.Collections;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public CardData[] cardDatas;
    public Card cardPrefab;
    public int handSize;
    public int rewardSize;

    public Hand hand;

    public void DrawCard(Transform parent, bool isReward)
    {
        int index = Random.Range(0, cardDatas.Length);
        Card newCard = Instantiate(cardPrefab, parent, false);
        newCard.isReward = isReward;
        newCard.data = cardDatas[index];
        newCard.RenderData();

        hand.AddCard(newCard);
    }

    public void DrawHand()
    {
        for (int i = 0; i < handSize; i++)
            DrawCard(hand.transform, false);
    }
}