using System;
using System.Collections;
using System.Collections.Generic;
using Core.Logging;
using UnityEngine;

public class DefensesShopPanelController : MonoBehaviour
{
    [SerializeField] Transform turretButtonsContainer;
    [SerializeField] BuyPlaceableItemButton buyPlaceableButtonPrefab;
    [SerializeField] List<PlaceableShopItemSO> turretShopItems;

    GameObject _heldItem;
    IPlaceable _placeable;

    public bool placingItem;

    readonly CLogger _logger = new(true);

    void Start()
    {
        foreach (PlaceableShopItemSO turretItem in turretShopItems)
        {
            BuyPlaceableItemButton buyPlaceableButton = Instantiate(buyPlaceableButtonPrefab, turretButtonsContainer, false);
            buyPlaceableButton.Init(turretItem, BuyPlaceable);
        }
    }

    void BuyPlaceable(PlaceableShopItemSO item)
    {
        if (_heldItem != null)
        {
            Destroy(_heldItem);
            _heldItem = null;
        }

        _heldItem = Instantiate(item.itemPrefab, MainCamera.Instance.ClampedMousePos, Quaternion.identity);

        if (!_heldItem.TryGetComponent(out _placeable))
        {
            _logger.LogError("Created object does not implement IPlaceable interface");
            return;
        }

        MoveItemToMouse(IsValidPlacement);
    }

    void MoveItemToMouse(Func<bool> placementCondition)
    {
        StartCoroutine(MoveItemToMouseRoutine(placementCondition));
    }

    IEnumerator MoveItemToMouseRoutine(Func<bool> placementCondition)
    {
        placingItem = true;

        _placeable.PickedUp();

        MainCamera cam = MainCamera.Instance;

        while (placementCondition?.Invoke() == false)
        {
            _heldItem.transform.position = cam.MousePos;
            yield return null;
        }

        _heldItem = null;
        placingItem = false;

        _placeable.Place(cam.MousePos);
    }

    bool IsValidPlacement()
    {
        if (Input.GetMouseButtonDown(0) && !BuyPlaceableItemButton.beingHovered) return true;
        return false;
    }
}
