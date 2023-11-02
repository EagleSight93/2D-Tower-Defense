using SpleenTween;
using System;
using System.Collections;
using System.Collections.Generic;
using Core.Logging;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEngine.Rendering.Universal;
using Unity.Burst.Intrinsics;

public class Hand : MonoBehaviour
{
    public int handSize;
    public Card selectedCard;
    public List<Card> cards;

   
    public float cardWidth;

    public float maxRotationAngle;
    public float cardPadding;
    public float slideUpHeight;
    public float height;

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

    private void OnValidate()
    {
        CalculatePositions();
        foreach (Card card in cards)
        {
            card.slideHeight = slideUpHeight;
            MoveCardImmediate(card);
            RotateCardImmediate(card);
        }
    }

    void MoveCard(Card card)
    {
        if (!card.isMoving)
        {
            card.currentMoveCoroutine = StartCoroutine(AnimateCardMove(card, animationTimeMove));
        }
    }

    void MoveCardImmediate(Card card)
    {
        if (card.currentMoveCoroutine != null) StopCoroutine(card.currentMoveCoroutine);
        card.isMoving = false;
        card.currentMoveCoroutine = StartCoroutine(AnimateCardMove(card, animationTimeMove));
    }


    void RotateCard(Card card)
    {
        if (!card.isRotating)
        {
            StartCoroutine(AnimateCardRotate(card, animationTimeRotate));
        }
    }

    void RotateCardImmediate(Card card)
    {
        if (card.currentRotateCoroutine != null) StopCoroutine(card.currentRotateCoroutine);
        card.isRotating = false;
        card.currentRotateCoroutine = StartCoroutine(AnimateCardRotate(card, animationTimeRotate));
    }

    //Set card anchors
    void CalculatePositions()
    {
        int cardCount = cards.Count;
        int midCardIndex = 0;

        if (cardCount == 1)
        {
            cards[0].anchorPos = Vector3.zero;
            cards[0].targetRotation = Quaternion.identity;
            midCardIndex = 1;
            return;
        }

        // Start X value of hand 
        float cursorX = 0; 

        if (cardCount % 2 != 0)
        {
            //if odd
            float cardHalf = (cardCount - 1) * 0.5f;
            midCardIndex = (int)cardHalf;//Need to solve this one
            cursorX = -cardHalf * (cardWidth + cardPadding);
        }
        else
        {
            //if even
            float cardHalf = cardCount* 0.5f;
            midCardIndex = (int)cardHalf;
            cursorX = (-cardHalf * (cardWidth + cardPadding)) + (cardWidth + cardPadding) * 0.5f;

        }

        //set target positions an rotations
        foreach (Card card in cards)
        {
            //Set Target Rotation
            /*
            card.transform.localRotation = Quaternion.identity;
            float angle = ((float)(cards.IndexOf(card) - midCardIndex)/midCardIndex) * -maxRotationAngle;
            card.targetRotation = Quaternion.Euler(0, 0, angle);
            */
            
            float angle = ((float)(cards.IndexOf(card) - midCardIndex)/midCardIndex) * -maxRotationAngle;
            card.targetRotation = Quaternion.Euler(0, 0, angle);
            

            //Set Target Position
            float xVal = cursorX;
            float yVal = height;

            card.anchorPos = new Vector3(xVal, yVal, 0);
            card.targetPos = card.anchorPos;

            cursorX += (cardWidth + cardPadding);
        }
    }


    public void AddCard(Card newCard)
    {
        cards.Add(newCard);
        newCard.slideHeight = slideUpHeight;
        CalculatePositions();
        foreach (Card card in cards)
        {
            RotateCardImmediate(card);
            MoveCardImmediate(card);
        }
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
        card.isRotating = true;
        float time = 0;
        Quaternion startRot = card.transform.localRotation;

        while (time <= animationTime)
        {
            Vector3 lerpRot = Vector3.Lerp(startRot.eulerAngles, card.targetRotation.eulerAngles, rotatingCardCurve.Evaluate(time/animationTime));
            card.transform.localRotation = Quaternion.Euler(lerpRot.x, lerpRot.y, lerpRot.z);
            time += Time.deltaTime;
            yield return null;
        }

        card.isRotating = false;
    }

    IEnumerator SlideUpCard(Card card)
    {
        yield return new WaitUntil(() => !card.isRotating);
        
    }

}
