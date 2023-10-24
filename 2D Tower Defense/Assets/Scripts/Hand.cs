using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Deck deck;
    public Card selectedCard;
    public List<Card> cards;

    void OnEnable()
    {
        CardEvents.OnCardClicked += PlayCard;
    }
    void OnDisable()
    {
        CardEvents.OnCardClicked -= PlayCard;
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    public void RemoveCard(Card card)
    {
        cards.Remove(card);

        // for testing, destroy the card, later it should be added back into the deck
        Destroy(card.gameObject);
    }

    public void PlayCard(Card card)
    {
        // some other stuff
        RemoveCard(card);
    }
}
