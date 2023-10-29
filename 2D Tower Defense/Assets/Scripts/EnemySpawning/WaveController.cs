using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public int currentWave;

    [ContextMenu("Start All Waves")]
    void StartAllWaves()
    {
        GameEvents.WaveStarted(currentWave);
        currentWave++;
    }
}
