using SpleenTween;
using System;
using System.Collections;
using System.Collections.Generic;
using Core.Logging;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Hand : MonoBehaviour
{
    public Card selectedCard;
    public List<Card> cards;

    public float cardHeight;
    public float cardWidth;

    public float rotationAngleMult;
    public float cardPadding;
    public float cardRaiseHeight;

    public float animationTimeMove;
    public float animationTimeRotate;
    public float animationTimeRaise;

    public AnimationCurve movingCardCurve;
    public AnimationCurve rotatingCardCurve;

    readonly CLogger _logger = new(true);

    void OnEnable()
    {
        CardEvents.OnCardClicked += SelectCard;
        CardEvents.OnCardEntered += MoveCardImmediate;
        CardEvents.OnCardExited += MoveCardImmediate;
    }
    void OnDisable()
    {
        CardEvents.OnCardClicked -= SelectCard;
        CardEvents.OnCardEntered -= MoveCardImmediate;
        CardEvents.OnCardExited -= MoveCardImmediate;
    }


    void MoveCard(Card card)
    {
        if (card.transform.localPosition != card.targetPos && !card.isMoving)
        {
            card.currentMoveCoroutine = StartCoroutine(AnimateCardMove(card, animationTimeMove));
        }
    }

    void MoveCardImmediate(Card card)
    {
        if (card.transform.localPosition != card.targetPos)
        {
            if (card.currentMoveCoroutine != null) StopCoroutine(card.currentMoveCoroutine);
            card.isMoving = false;
            card.currentMoveCoroutine = StartCoroutine(AnimateCardMove(card, animationTimeMove));
        }
    }


    void RotateCards()
    {
        foreach (Card card in cards)
        {
            if (card.transform.localRotation != card.targetRotation)
            {
                card.isRotating = true;
                StartCoroutine(AnimateCardRotate(card, animationTimeRotate));
            }
        }
    }

    //Set card anchors
    void CalculatePositions()
    {
        if (cards.Count == 1)
        {
            cards[0].anchorPos = Vector3.zero;
            cards[0].targetRotation = Quaternion.identity;
            return;
        }

        // Start X value of hand 
        float cursor = 0; 

        if (cards.Count % 2 != 0)
        {
            //if odd
            float cardHalf = (cards.Count - 1) * 0.5f;
            cursor = -cardHalf * (cardWidth + cardPadding);
        }
        else
        {
            //if even
            float cardHalf = cards.Count * 0.5f;
            cursor = (-cardHalf * (cardWidth + cardPadding)) + (cardWidth + cardPadding) * 0.5f;
        }

        //set target positions an rotations
        foreach (Card card in cards)
        {
            //Set Position
            float xVal = cursor;
            float yVal = -Mathf.Abs(xVal)/7.5f; 
            card.anchorPos = new Vector3 (xVal, 0, 0);
            card.targetPos = card.anchorPos;

            cursor += (cardWidth + cardPadding);

            //Set Rotation
            float angle = -rotationAngleMult * card.targetPos.x /1000;
            card.targetRotation = Quaternion.Euler(0,0, angle);
        }
    }


    public void AddCard(Card newCard)
    {
        cards.Add(newCard);
        //set height when entered and maybe rotation angle?
        newCard.slideHeight = cardRaiseHeight;
        CalculatePositions();
        foreach (Card card in cards)
        {
            MoveCardImmediate(card);
        }  
        //RotateCards();
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


    IEnumerator AnimateCardMove(Card card, float animationTime)
    {
        card.isMoving = true;
        float time = 0;
        Vector3 startPos = card.transform.localPosition;
        Vector3 target = card.targetPos;

        while (time <= animationTime)
        {
            Vector3 lerpPos = Vector3.Lerp(startPos, target, movingCardCurve.Evaluate(time/animationTime));
            card.transform.localPosition = lerpPos;
            time += Time.deltaTime;
            yield return null;
        }

        card.isMoving = false;
        //_logger.Log("Coroutine END", LogColor.Black);
    }



    IEnumerator AnimateCardRotate(Card card, float animationTime)
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

        card.isRotating = false;
    }
}
