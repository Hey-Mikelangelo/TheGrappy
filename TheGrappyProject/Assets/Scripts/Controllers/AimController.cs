using UnityEngine;

public abstract class AimController : PlayerModuleController
{
    public Vector2 AimPointPosition { get; protected set; }
    public Vector2 AimUIDirection { get; protected set; }
    public bool HasTarget { get; protected set; }

    public virtual void InitAimPoint(Vector2 prevAimPoint)
    {
        AimPointPosition = prevAimPoint;
    }
}
