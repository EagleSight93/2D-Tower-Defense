using System;
using UnityEngine;

public static class CombatEvents
{
    public static event Action<Turret> OnTurretShot;
    public static void TurretShot(Turret turret) => OnTurretShot?.Invoke(turret);
}
