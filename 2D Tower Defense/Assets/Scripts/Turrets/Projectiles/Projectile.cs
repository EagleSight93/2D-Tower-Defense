using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float _speed;
    float _damage;
    float _lifetime;
    Action<IDestructable> _onHit;

    Rigidbody2D _rb;

    public void Init(float damage, float speed, float lifetime, Action<IDestructable> onHit)
    {
        _rb = GetComponent<Rigidbody2D>();

        _damage = damage;
        _speed = speed;
        _lifetime = lifetime;
        _onHit = onHit;

        _rb.velocity = transform.right * _speed;

        StartCoroutine(DestroyAfterLifetimeComplete());
    }

    IEnumerator DestroyAfterLifetimeComplete()
    {
        for (float time = 0; time < _lifetime; time += Time.deltaTime)
        {
            yield return null;
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IDestructable destructableObj)) return;

        destructableObj.Damaged(_damage);
        _onHit?.Invoke(destructableObj);
    }
}
