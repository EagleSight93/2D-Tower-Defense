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
        CardEvents.OnCardClicked += SelectCard;
    }
    void OnDisable()
    {
        CardEvents.OnCardClicked -= SelectCard;
    }

    private void Update()
    {
        
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

    public void SelectCard(Card card)
    {
       selectedCard = card;
    }

    public void UseCard(Card card)
    {
        // some other stuff
        RemoveCard(card);
    }
}
