using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensesShopPanelController : MonoBehaviour
{
    [SerializeField] Transform turretButtonsContainer;
    [SerializeField] BuyPlaceableItemButton buyPlaceableButtonPrefab;
    [SerializeField] List<PlaceableShopItemSO> turretShopItems;
    void Start()
    {
        foreach (PlaceableShopItemSO turretItem in turretShopItems)
        {
            BuyPlaceableItemButton buyPlaceableButton = Instantiate(buyPlaceableButtonPrefab, turretButtonsContainer, false);
            buyPlaceableButton.Init(turretItem.itemPrefab, turretItem.title, turretItem.cost.ToString(), turretItem.icon);
        }
    }
}
