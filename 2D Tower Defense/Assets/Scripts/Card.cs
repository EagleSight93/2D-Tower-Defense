using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public CardData data;
    [SerializeField] TMP_Text cost;
    [SerializeField] TMP_Text cardName;
    [SerializeField] TMP_Text description;

    [HideInInspector] public bool isReward;

    public float slideHeight;
    public Vector3 anchorPos;
    public Vector3 targetPos;
    public Quaternion targetRotation;

    public bool isMoving = false;
    public bool isRotating = false;

    public Coroutine currentMoveCoroutine;

    bool _playerIsHoveringThisCard = false;

    public void RenderData()
    {
        cost.text = data.cost.ToString();
        cardName.text = data.cardName;
        description.text = data.description;
    }

    void Update()
    {
        if (_playerIsHoveringThisCard)
        {
            CardEvents.HoveringCard(this);
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        CardEvents.ClickedCard(this);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _playerIsHoveringThisCard = true;
        targetPos = anchorPos + (Vector3.up * slideHeight);
        CardEvents.EnteredCard(this);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        _playerIsHoveringThisCard = false;
        targetPos = anchorPos;
        CardEvents.ExitedCard(this);
    }
}
