using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDestructable, ITargetable
{
    void IDestructable.Damaged(float damageTaken) { }
    void IDestructable.Destroyed() => Destroy(gameObject);

    Vector3 ITargetable.GetPosition() => transform.position;
    bool ITargetable.IsTargetable() => true;
}
