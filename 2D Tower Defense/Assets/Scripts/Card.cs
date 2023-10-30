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

    public Vector3 targetPos;
    public Quaternion targetRotation;

    public bool isMoving = false;
    public bool isRotating = false;

    public void RenderData()
    {
        cost.text = data.cost.ToString();
        cardName.text = data.cardName;
        description.text = data.description;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        CardEvents.ClickedCard(this);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        targetPos =  transform.localPosition + transform.up * 100;
        CardEvents.EnterCard(this);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        targetPos =  transform.localPosition + transform.up * -100;
        CardEvents.ExitedCard(this);
    }
}
