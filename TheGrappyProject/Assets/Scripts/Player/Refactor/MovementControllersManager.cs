using UnityEngine;
using Zenject;

public class MovementControllersManager : PlayerModuleControllerManager<MovementController>
{
    [SerializeField]
    private ForwardMovementController forwardMovementController;
    [SerializeField]
    private OrbitingMovementController grapMovementController;

    protected override void FillControllersList()
    {
        controllers.Add(grapMovementController);
        controllers.Add(forwardMovementController);
    }
}
