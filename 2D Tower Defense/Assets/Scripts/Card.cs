using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public CardData data;
    [SerializeField] TMP_Text cost;
    [SerializeField] TMP_Text cardName;
    [SerializeField] TMP_Text description;

    [HideInInspector] public bool isReward;

    public void RenderData()
    {
        cost.text = data.cost.ToString();
        cardName.text = data.cardName;
        description.text = data.description;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        CardEvents.ClickedCard(this);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        CardEvents.HoveredCard(this);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        CardEvents.ExitedCard(this);
    }
}
