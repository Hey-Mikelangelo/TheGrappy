using UnityEngine;

public class AimControllersManager : PlayerModuleControllerManager<AimController>
{
    [SerializeField] private WallAimController wallAimController;
    [SerializeField] private SimpleAimController simpleAimController;

    public override void SwitchToController<T>()
    {
        Vector2 prevAimPointPosition = Vector2.zero;
        if (ActiveController != null)
        {
            prevAimPointPosition = ActiveController.AimPointPosition;
        }
        base.SwitchToController<T>();
        ActiveController.InitAimPoint(prevAimPointPosition);
    }

    protected override void FillControllersList()
    {
        controllers.Add(wallAimController);
        controllers.Add(simpleAimController);
    }
}
