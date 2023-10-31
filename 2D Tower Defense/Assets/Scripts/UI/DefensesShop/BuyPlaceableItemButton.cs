using System;
using System.Collections;
using System.Collections.Generic;
using Core.Logging;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyPlaceableItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool beingHovered;
    static bool _placingItem;

    GameObject _itemPrefab;
    GameObject _heldItem;

    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text costText;
    [SerializeField] Image spriteIcon;
    [SerializeField] Button purchaseButton;
    IPlaceable _placeable;

    readonly CLogger _logger = new(true);

    void OnDisable()
    {
        purchaseButton.onClick.RemoveAllListeners();
    }

    public void Init(PlaceableShopItemSO item, Action<PlaceableShopItemSO> onClick)
    {
        if (item.itemPrefab.GetComponent<IPlaceable>() == null)
        {
            _logger.LogError("The object you are placing does not implement the IPlaceable interface", gameObject, LogColor.Red);
            return;
        }

        _itemPrefab = item.itemPrefab;
        titleText.text = item.title;
        costText.text = item.cost.ToString();
        spriteIcon.sprite = item.icon;

        purchaseButton.onClick.AddListener(() => onClick?.Invoke(item));
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        beingHovered = true;
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        beingHovered = false;
    }
}
