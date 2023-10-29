using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Vector3 _target;
    Rigidbody2D _rb;
    [SerializeField] float speed = 250f;
    [SerializeField] float drag = 1f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _rb.drag = drag;
        _target = Vector3.zero;
    }

    void FixedUpdate()
    {
        Vector3 dir = (_target - transform.position).normalized;
        _rb.AddForce(dir * (speed * Time.fixedDeltaTime));
    }
}
