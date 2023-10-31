using System;
using UnityEngine;

public class ShopEvents : MonoBehaviour
{
    public static event Action<int> OnItemPurchased;

    public static void WaveNumberStarted(int goldCost) => OnItemPurchased?.Invoke(goldCost);
}
