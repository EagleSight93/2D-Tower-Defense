using System;

public static class GameEvents
{
    public static event Action OnGameStart;
    public static void GameStarted() => OnGameStart?.Invoke();

    public static event Action<int> OnWaveStarted;
    public static void WaveStarted(int waveNumber) => OnWaveStarted?.Invoke(waveNumber);
}
