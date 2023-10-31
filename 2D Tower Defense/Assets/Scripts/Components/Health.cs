using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IDestructable))]
public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;

    public float CurrentHealth
    {
        get => currentHealth; 
        private set => currentHealth = Mathf.Max(0f, value);
    }

    IDestructable _destructable;

    void Awake()
    {
        _destructable = GetComponent<IDestructable>();
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        _destructable.Damaged(damage);

        if (CurrentHealth <= 0)
        {
            _destructable.Destroyed();
        }
    }
}
