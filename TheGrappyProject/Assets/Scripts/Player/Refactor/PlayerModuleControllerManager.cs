using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerModuleControllerManager<ModuleController> : MonoBehaviour
    where ModuleController : PlayerModuleController
{
    [SerializeField] protected Player player;
    public ModuleController ActiveController { get; private set; }
    protected List<ModuleController> controllers = new List<ModuleController>();
    public void DisableModule()
    {
        if (ActiveController != null)
        {
            ActiveController.Disable();
            ActiveController = null;
        }
    }

    public void SwitchToController<T>() where T : ModuleController
    {
        ModuleController newActiveController = controllers.Find(x => x is T);
        if (newActiveController == null)
        {
            Debug.LogError($"controller of type {typeof(T)} not found");
            return;
        }

        if (ActiveController != null && ActiveController.GetType() == newActiveController.GetType())
        {
            return;
        }
        DisableModule();
        ActiveController = newActiveController;
        ActiveController.Enable();
    }

    protected virtual void Awake()
    {
        FillControllersList();
        foreach (ModuleController playerModuleController in controllers)
        {
            playerModuleController.SetPlayer(player);
        }
    }

    protected void Update()
    {
        if (ActiveController != null)
        {
            ActiveController.RunUpdate();
        }
    }

    protected void FixedUpdate()
    {
        if (ActiveController != null)
        {
            ActiveController.RunFixedUpdate();
        }
    }

    protected abstract void FillControllersList();
    private enum UpdateType
    {
        Update = 1,
        FixedUpdate = 2
    }
}
