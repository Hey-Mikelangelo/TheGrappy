using UnityEngine;
using Zenject;

public class AimControllersManager : PlayerModuleControllerManager<AimController>
{
    [SerializeField]
    private WallAimController wallAimController;

    protected override void FillControllersList()
    {
        controllers.Add(wallAimController);
    }
}
