using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public FollowPath pathfinding;

    private void Awake()
    {
        pathfinding = GetComponent<FollowPath>();
    }
}
