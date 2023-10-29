using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public int currentWave = 1;

    void Start()
    {
        StartAllWaves();
    }

    [ContextMenu("Start All Waves")]
    void StartAllWaves()
    {
        GameEvents.WaveStarted();
        GameEvents.WaveNumberStarted(currentWave);
        currentWave++;
    }
}
