using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy/Wave")]
public class EnemyWave : ScriptableObject
{
    public float duration = 60f;
    public List<EnemyGroup> enemyGroups;
}
