using SpleenTween;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Card selectedCard;
    public List<Card> cards;

    public float cardheight;
    public float cardwidth;
    public float rotationAngleMult;
    public float padding;

    public float animationTimeMove;
    public float animationTimeRotate;
    public float animationTimeRaise;

    public AnimationCurve movingCardCurve;
    public AnimationCurve rotatingCardCurve;


    void OnEnable()
    {
        CardEvents.OnCardClicked += SelectCard;
        CardEvents.OnCardEnter += (card) => MoveCard(card, animationTimeRaise);
        CardEvents.OnCardExited += (card) => MoveCard(card, animationTimeRaise);
    }
    void OnDisable()
    {
        CardEvents.OnCardClicked -= SelectCard;
        CardEvents.OnCardEnter -= (card) => MoveCard(card, animationTimeRaise);
        CardEvents.OnCardExited -= (card) => MoveCard(card, animationTimeRaise);
    }

    void MoveCard(Card card ,float animationTime)
    {
        if (card.transform.localPosition != card.targetPos && !card.isMoving)
        {
            card.isMoving = true;
            StartCoroutine(AnimateMoveCard(card, animationTime));
        }
    }

    void RotateCards()
    {
        foreach (Card card in cards)
        {
            if (card.transform.localRotation != card.targetRotation && !card.isRotating)
            {
                card.isRotating = true;
                StartCoroutine(AnimateRotateCard(card, animationTimeRotate));
            }
        }
    }

    void CalculatePositions()
    {
        if (cards.Count == 1)
        {
            cards[0].targetPos = Vector3.zero;
            cards[0].targetRotation = Quaternion.identity;
            return;
        }

        int totalCards = cards.Count;
        float cardHalf = totalCards * 0.5f;

        float increment = -cardHalf * cardwidth; 

        foreach (Card card in cards)
        {
            float xVal = increment + cardwidth/2;
            float yVal = -Mathf.Abs(xVal)/7.5f; 
            card.targetPos = new Vector3 (xVal, yVal, 0);

            increment += cardwidth;

            float angle = -rotationAngleMult * card.targetPos.x /1000;
            card.targetRotation = Quaternion.Euler(0,0, angle);

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


    public void AddCard(Card newCard)
    {
        cards.Add(newCard);
        CalculatePositions();
        foreach (Card card in cards)
        {
            MoveCard(card,animationTimeMove);
        }  
        RotateCards();
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


    IEnumerator AnimateMoveCard(Card card,float animationTime)
    {
        
        float time = 0;
        Vector3 startPos = card.transform.localPosition;

        while (time <= animationTime)
        {
            Vector3 lerpPos = Vector3.Lerp(startPos, card.targetPos, movingCardCurve.Evaluate(time));
            card.transform.localPosition = lerpPos;
            time += Time.deltaTime;
            yield return null;
        }

        card.isMoving=false;
        
    }


    IEnumerator AnimateRotateCard(Card card, float animationTime)
    {
        float time = 0;
        Quaternion startRot = card.transform.localRotation;

        while (time <= animationTime)
        {
            Vector3 lerpRot = Vector3.Lerp(startRot.eulerAngles, card.targetRotation.eulerAngles, rotatingCardCurve.Evaluate(time));
            card.transform.localRotation = Quaternion.Euler(lerpRot.x, lerpRot.y, lerpRot.z);
            time += Time.deltaTime;
            yield return null;
        }

        card.isRotating=false;
    }
}
