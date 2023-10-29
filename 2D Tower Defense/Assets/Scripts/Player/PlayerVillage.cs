using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVillage : MonoBehaviour
{
    public float health = 100f;
    public int currency = 100;
    public float maxMana = 5f;

    float _mana;
    public float Mana
    {
        get => Mathf.Clamp(_mana, 0, maxMana);
        set => _mana = value;
    }
}
