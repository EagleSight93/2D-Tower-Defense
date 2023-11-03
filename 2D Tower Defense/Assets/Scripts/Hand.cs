using SpleenTween;
using System;
using System.Collections;
using System.Collections.Generic;
using Core.Logging;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

    [SerializeField] Ease movingCardEase = Ease.OutSine;
    [SerializeField] Ease rotatingCardEase = Ease.OutSine;

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

    void OnValidate()
    {
        CalculatePositions();
        foreach (Card card in cards)
        {
            if (card == null) continue;

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

        if (cardCount == 1)
        {
            cards[0].anchorPos = Vector3.zero;
            cards[0].targetRotation = Quaternion.identity;
            return;
        }
        float totalCardHalf = cardCount * 0.5f;

        float handExtents = cardCount * (cardWidth + cardPadding) / 2;
        


        //set target positions an rotations
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            if (card == null) continue;


            if (cards.Count == handSize)
            {
                print("JH");
            }   

            float lerpVal = (i - totalCardHalf) / (cardCount - totalCardHalf);
            float sign = Mathf.Sign(i - totalCardHalf);
            float targetAngle = -sign * maxRotationAngle;

            card.targetRotation = Quaternion.Slerp(card.transform.localRotation, Quaternion.Euler(0, 0, targetAngle), Mathf.Abs(lerpVal));

            float targetPos = sign * handExtents;
            card.anchorPos = Vector3.Slerp(card.transform.localPosition, new Vector3(targetPos, -height, 0), Mathf.Abs(lerpVal));
            card.targetPos = card.anchorPos;
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
            float lerpVal = Easing.EaseVal(time / animationTime, movingCardEase);
            Vector3 lerpPos = Vector3.Lerp(startPos, target, lerpVal);
            card.transform.localPosition = lerpPos;
            time += Time.deltaTime;
            yield return null;
        }

        card.transform.localPosition = target;

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
            float lerpVal = Easing.EaseVal(time / animationTime, rotatingCardEase);
      
            card.transform.localRotation = Quaternion.Slerp(card.transform.localRotation, card.targetRotation, lerpVal);
            time += Time.deltaTime;
            yield return null;
        }

        card.transform.localRotation = card.targetRotation;
        card.isRotating = false;
    }

    IEnumerator SlideUpCard(Card card)
    {
        yield return new WaitUntil(() => !card.isRotating);
        
    }

}
