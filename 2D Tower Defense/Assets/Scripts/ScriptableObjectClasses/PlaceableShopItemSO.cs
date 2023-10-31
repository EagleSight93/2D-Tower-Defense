using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ShopItems/PlaceableItem")]
public class PlaceableShopItemSO : ScriptableObject
{
    public GameObject itemPrefab;
    public Sprite icon;
    public string title;
    public int cost;
}
