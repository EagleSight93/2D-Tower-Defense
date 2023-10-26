using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    List<Vector3> _waypoints = new();
    int _curWaypointIndex;
    Vector3 TargetPoint => _waypoints[Mathf.Min(_curWaypointIndex, _waypoints.Count - 1)];

    [SerializeField] float speed = 250f;
    [SerializeField] float waypointDistThreshold = 0.1f;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(List<Vector3> waypoints)
    {
        _waypoints = waypoints;
        transform.position = _waypoints[0];
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 dir = (TargetPoint - transform.position).normalized;
        rb.AddForce(dir * (speed * Time.deltaTime));
        if(Vector3.Distance(transform.position, TargetPoint) <= waypointDistThreshold)
        {
            _curWaypointIndex++;
        }
    }
}
