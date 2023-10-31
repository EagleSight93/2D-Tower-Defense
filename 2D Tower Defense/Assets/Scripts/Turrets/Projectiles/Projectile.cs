using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float _speed;
    float _damage;
    float _lifetime;
    int _pierces;
    Action<CombatEntity, Projectile> _onHit;
    Action _onDestroy;

    Rigidbody2D _rb;

    public void Init(float damage, float speed, float lifetime, int pierces, Action<CombatEntity, Projectile> onHit, Action onDestroy)
    {
        _rb = GetComponent<Rigidbody2D>();

        _damage = damage;
        _speed = speed;
        _lifetime = lifetime;
        _pierces = pierces;

        _onHit = onHit;
        _onDestroy = onDestroy;

        _rb.velocity = transform.right * _speed;

        StartCoroutine(DestroyAfterLifetimeComplete());
    }

    IEnumerator DestroyAfterLifetimeComplete()
    {
        for (float time = 0; time < _lifetime; time += Time.deltaTime)
        {
            yield return null;
        }
        _onDestroy?.Invoke();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out CombatEntity entity)) return;
        _onHit?.Invoke(entity, this);

        _pierces--;
        if (_pierces <= 0)
        {
            _onDestroy?.Invoke();
            Destroy(gameObject);
        }
    }
}
