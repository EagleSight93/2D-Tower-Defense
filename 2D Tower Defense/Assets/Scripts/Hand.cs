using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Card selectedCard;
    public List<Card> cards;

    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    public void RemoveCard(Card card)
    {
        cards.Remove(card);
    }

    public void PlayCard(Card card)
    {
        // some other stuff
        RemoveCard(card);
    }
}
