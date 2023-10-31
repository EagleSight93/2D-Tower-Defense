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
    static bool _beingHovered;
    static bool _placingItem;

    GameObject _itemPrefab;
    GameObject _heldItem;

    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text costText;
    [SerializeField] Image spriteIcon;
    [SerializeField] Button purchaseButton;
    IPlaceable _placeable;

    readonly CLogger _logger = new(true);

    void OnEnable()
    {
        purchaseButton.onClick.AddListener(CreatePlaceableObject);
    }
    void OnDisable()
    {
        purchaseButton.onClick.RemoveAllListeners();
    }

    public void Init(GameObject itemPrefab, string title, string cost, Sprite icon)
    {
        if (itemPrefab.GetComponent<IPlaceable>() == null)
        {
            _logger.LogError("The object you are placing does not implement the IPlaceable interface", gameObject, LogColor.Red);
            return;
        }
        _itemPrefab = itemPrefab;
        titleText.text = title;
        costText.text = cost;
        spriteIcon.sprite = icon;
    }

    public void CreatePlaceableObject()
    {
        if (_placingItem) return;

        if (_heldItem != null)
        {
            Destroy(_heldItem);
        }

        _heldItem = Instantiate(_itemPrefab, MainCamera.Instance.ClampedMousePos, Quaternion.identity);

        if (!_heldItem.TryGetComponent(out _placeable))
        {
            _logger.LogError("Created object does not implement IPlaceable interface");
            return;
        }

        MoveItemToMouse(IsValidPlacement);
    }

    void MoveItemToMouse(Func<bool> placementCondition) => StartCoroutine(MoveItemToMouseRoutine(placementCondition));

    IEnumerator MoveItemToMouseRoutine(Func<bool> placementCondition)
    {
        _placingItem = true;

        _placeable.PickedUp();

        MainCamera cam = MainCamera.Instance;

        while (placementCondition?.Invoke() == false)
        {
            _heldItem.transform.position = cam.MousePos;
            yield return null;
        }

        _heldItem = null;
        _placingItem = false;

        _placeable.Place(cam.MousePos);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _beingHovered = true;
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        _beingHovered = false;
    }

    bool IsValidPlacement()
    {
        if (Input.GetMouseButtonDown(0) && !_beingHovered) return true;
        return false;
    }
}
