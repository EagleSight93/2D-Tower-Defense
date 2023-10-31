using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class CombatEntity : MonoBehaviour, IDestructable
{
    public bool playerOwned;
    public Health health;

    public static bool AreEnemies(CombatEntity first, CombatEntity second)
    {
        return (first.playerOwned && !second.playerOwned) ||
               (!first && second.playerOwned);
    }

    void IDestructable.Damaged(float damage)
    {
        
    }
    void IDestructable.Destroyed()
    {
        Destroy(gameObject);
    }
}
