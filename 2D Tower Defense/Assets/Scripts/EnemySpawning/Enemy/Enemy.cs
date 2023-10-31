using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CombatEntity))]
public class Enemy : MonoBehaviour, IDestructable, ITargetable
{
    NavMeshAgent _agent;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    void Start()
    {
        _agent.SetDestination(Vector3.zero);
    }

    void IDestructable.Damaged(float damageTaken) { }
    void IDestructable.Destroyed() => Destroy(gameObject);

    Vector3 ITargetable.GetPosition() => transform.position;
    bool ITargetable.IsTargetable() => true;
}
