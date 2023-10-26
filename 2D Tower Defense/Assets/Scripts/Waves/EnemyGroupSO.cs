using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGroup", menuName = "ScriptableObjects/EnemyGroup")]
public class EnemyGroupSO : ScriptableObject
{
    public List<Enemy> enemies;
    public float spawnInterval;
}
