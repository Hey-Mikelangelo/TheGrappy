using UnityEngine;

public abstract class AimController : PlayerModuleController
{
    public Vector2 AimPointPosition { get; protected set; }
    public bool HasTarget { get; protected set; }
}
