using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Turret : MonoBehaviour, IDestructable, ITargetable
{
    public enum DetectionType
    {
        Closest,
        Furthest,
    }

    [Header("Combat")]
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] float damage = 10f;
    [SerializeField] float atkInterval = 0.25f;
    [SerializeField] float accurateAngle = 30f;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float projectileLifetime = 1f;

    [Header("Detection")]
    [SerializeField] LayerMask targetMask;
    [SerializeField] DetectionType detectionType;
    [SerializeField] float visionRadius = 3f;
    [SerializeField] float detectionInterval = 0.5f;
    [SerializeField] bool detectTargets;

    [SerializeField] Transform shootPos;

    ITurret _turret;

    float _curAtkTime;
    float _curDetectionTime;

    void Awake()
    {
        _turret = GetComponent<ITurret>();
    }

    void OnEnable()
    {
        GameEvents.OnWaveStarted += EnableTargetDetection;
    }
    void OnDisable()
    {
        GameEvents.OnWaveEnded += DisableTargetDetection;
    }

    void Update()
    {
        ITargetable target = FindTarget();

        if (target is null) return;

        FaceTarget(target);
        AttackInDirection();
    }

    void AttackInDirection()
    {
        _curAtkTime += Time.deltaTime;
        if (_curAtkTime < atkInterval) return;

        //_turret.AttackTarget(target);

        float halfAccurateAngle = accurateAngle * 0.5f;
        float randAngle = Random.Range(transform.eulerAngles.z - halfAccurateAngle, transform.eulerAngles.z + halfAccurateAngle);
        Quaternion targetRotation = Quaternion.Euler(0, 0, randAngle);

        var projectile = Instantiate(projectilePrefab, shootPos.position, targetRotation);
        projectile.Init(projectileSpeed, damage, projectileLifetime, (_) => print("Hit Enemy"));

        _curAtkTime = 0;
    }

    void FaceTarget(ITargetable target)
    {
        Vector3 targetDir = (target.GetPosition() - transform.position).normalized;
        float targetAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    ITargetable FindTarget()
    {
        if (!detectTargets) return null;

        _curDetectionTime += Time.deltaTime;
        if (_curDetectionTime < detectionInterval) return null;

        _curDetectionTime = 0f;
        return detectionType switch
        {
            DetectionType.Closest => GetClosestTarget(),
            DetectionType.Furthest => GetFurthestTarget(),
            _ => null
        };
    }

    IEnumerable<ITargetable> GetPotentialTargets()
    {
        RaycastHit2D[] targetsHit = Physics2D.CircleCastAll(transform.position, visionRadius, Vector3.zero, 0, targetMask);
        ITargetable[] targets = targetsHit
            .Select(hit => hit.transform.GetComponent<ITargetable>())
            .Where(target => target.IsTargetable())
            .ToArray();
        return targets;
    }

    ITargetable GetClosestTarget()
    {
        IEnumerable<ITargetable> potentialTargets = GetPotentialTargets();
        float closestDist = float.MaxValue;
        ITargetable closestTarget = null;
        foreach (ITargetable target in potentialTargets)
        {
            float dist = Vector2.Distance(target.GetPosition(), transform.position);
            if (dist >= closestDist) continue;

            closestDist = dist;
            closestTarget = target;
        }
        return closestTarget;
    }

    ITargetable GetFurthestTarget()
    {
        IEnumerable<ITargetable> potentialTargets = GetPotentialTargets();
        float furthestDist = 0;
        ITargetable furthestTarget = null;
        foreach (ITargetable target in potentialTargets)
        {
            float dist = Vector2.Distance(target.GetPosition(), transform.position);
            if (dist <= furthestDist) continue;

            furthestDist = dist;
            furthestTarget = target;
        }
        return furthestTarget;
    }

    void EnableTargetDetection() => detectTargets = true;
    void DisableTargetDetection() => detectTargets = false;

    void IDestructable.Damaged(float damageTaken) { }
    void IDestructable.Destroyed() => Destroy(gameObject);

    Vector3 ITargetable.GetPosition() => transform.position;
    bool ITargetable.IsTargetable() => true;

    void OnDrawGizmos()
    {
        if (!shootPos)
        {
            Debug.LogError($"Assign a shootPos to {gameObject.name}", gameObject);
            return;
        }

        float projectileDist = projectileLifetime * projectileSpeed;
        Vector3 forwardRayPt = shootPos.position + transform.right * projectileDist;
        Vector3 leftRayPt = shootPos.position + Quaternion.Euler(0, 0, accurateAngle / 2) * transform.right * projectileDist;
        Vector3 rightRayPt = shootPos.position + Quaternion.Euler(0, 0, -(accurateAngle / 2)) * transform.right * projectileDist;

        Debug.DrawLine(shootPos.position, forwardRayPt, Color.red);
        Debug.DrawLine(shootPos.position, leftRayPt, Color.red);
        Debug.DrawLine(shootPos.position, rightRayPt, Color.red);

        Debug.DrawLine(forwardRayPt, leftRayPt, Color.red);
        Debug.DrawLine(forwardRayPt, rightRayPt, Color.red);

        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
