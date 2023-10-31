using System;
using System.Collections;
using System.Collections.Generic;
using Core.Logging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyPlaceableItemButton : MonoBehaviour
{
    GameObject _itemPrefab;
    GameObject _heldItem;

    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text costText;
    [SerializeField] Image spriteIcon;
    [SerializeField] Button purchaseButton;
    IPlaceable _placeable;

    CLogger _logger = new(true);

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

        StartCoroutine(MoveToMouseWhileTrue(() => Input.GetMouseButtonDown(0)));
    } 

    IEnumerator MoveToMouseWhileTrue(Func<bool> stopIfConditionMet)
    {
        _placeable.PickedUp();
        MainCamera cam = MainCamera.Instance;
        while (stopIfConditionMet?.Invoke() == false)
        {
            _heldItem.transform.position = cam.MousePos;
            yield return null;
        }

        _heldItem = null;
        _placeable.Place(cam.MousePos);
    }
}
