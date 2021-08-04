using UnityEngine;

public class AbilityControllersManager : PlayerModuleControllerManager<AbilityController>
{
    [SerializeField] private SideBoostController sideBoostController;
    protected override void FillControllersList()
    {
        controllers.Add(sideBoostController);
    }
}
