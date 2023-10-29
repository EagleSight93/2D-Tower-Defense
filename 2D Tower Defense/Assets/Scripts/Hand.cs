using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Card selectedCard;
    public List<Card> cards;

    public float cardheight;
    public float cardwidth;
    public float rotationAngleMult;
    public float padding;

    public float animationTime;

    public AnimationCurve movingCardCurve;
    public AnimationCurve rotatingCardCurve;


    void OnEnable()
    {
        CardEvents.OnCardClicked += SelectCard;
    }
    void OnDisable()
    {
        CardEvents.OnCardClicked -= SelectCard;
    }


    void MoveCards()
    {
        foreach (Card card in cards) {
            if(card.transform.localPosition != card.targetPos && !card.isMoving)
            {
                card.isMoving = true;
                StartCoroutine(MoveCard(card,animationTime));
            }
        }
    }


    void CalculatePositions()
    {
        if (cards.Count == 1)
        {
            cards[0].targetPos = Vector3.zero;
            cards[0].targetRotation = Vector3.zero;
            return;
        }

        int totalCards = cards.Count;
        float cardHalf = totalCards * 0.5f;

        float increment = -cardHalf * cardwidth; 

        foreach (Card card in cards)
        {
            card.targetPos = new Vector3 ( increment + cardwidth/2,0,0);

            increment += cardwidth;

            float angle = (card.targetPos.x - transform.position.x) * rotationAngleMult * Mathf.Sign(card.targetPos.x);
            card.transform.eulerAngles = new Vector3 (0,0,angle);

        }

        /*if (cards.Count % 2 != 0)
        {

        }
        else
        {

        }




        if (cards.Count > 1)
        {




            foreach (Card card in cards)
            {

            }
        }
        else
        {

        }*/
    }


    public void AddCard(Card card)
    {
        cards.Add(card);
        CalculatePositions();
        MoveCards();
    }

    public void RemoveCard(Card card)
    {
        cards.Remove(card);

        // for testing, destroy the card, later it should be added back into the deck
        Destroy(card.gameObject);
        CalculatePositions();
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


    IEnumerator MoveCard(Card card,float animationTime)
    {
        print("Started Coroutine");
        
        float time = 0;
        Vector3 startPos = card.transform.localPosition;

        while (time <= animationTime)
        {
            Vector3 lerpPos = Vector3.Lerp(startPos, card.targetPos, movingCardCurve.Evaluate(time));
            card.transform.localPosition = lerpPos;
            time += Time.deltaTime;
            print("animating");
            yield return null;
        }

        card.isMoving=false;
        
    }


    IEnumerator RotateCard(Card card, float animationTime)
    {
        //float time = 0;

        yield return null;
    }
}
