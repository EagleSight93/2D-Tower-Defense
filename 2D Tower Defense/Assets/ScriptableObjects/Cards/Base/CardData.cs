using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/Cards", order = 3)]
public class CardData : ScriptableObject
{
    public int id;
    public int rarity;
    public int cost;
    public string cardName;
    [TextArea] public string description;
}
