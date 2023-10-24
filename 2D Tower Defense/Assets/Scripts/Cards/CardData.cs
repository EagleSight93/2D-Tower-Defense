using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardData : ScriptableObject
{
    public int ID;
    public int rarity;
    public int cost;
    public string cardName;
    public string description;
    public GameObject turret;
    public Upgrade.UpgradeType upgradeType;
}
