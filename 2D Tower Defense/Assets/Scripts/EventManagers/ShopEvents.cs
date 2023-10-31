using System;
using UnityEngine;

public static class ShopEvents
{
    public static event Action<int> OnItemPurchased;

    public static void WaveNumberStarted(int goldCost) => OnItemPurchased?.Invoke(goldCost);
}
