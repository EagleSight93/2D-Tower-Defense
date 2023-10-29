using System;

public static class GameEvents
{
    public static event Action OnGameStart;
    public static event Action<int> OnWaveNumberStarted;
    public static event Action OnWaveStarted;
    public static event Action OnWaveEnded;


    public static void GameStarted() => OnGameStart?.Invoke();
    public static void WaveNumberStarted(int waveNumber) => OnWaveNumberStarted?.Invoke(waveNumber);
    public static void WaveStarted() => OnWaveStarted?.Invoke();
    public static void WaveEnded() => OnWaveEnded?.Invoke();
}
