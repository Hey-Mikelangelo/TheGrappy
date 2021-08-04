using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class PlayerModuleControllerManager<ModuleController> : MonoBehaviour
    where ModuleController : PlayerModuleController
{
    [Inject] protected Player player;
    public ModuleController ActiveController { get; private set; }
    protected List<ModuleController> controllers = new List<ModuleController>();
    private WaitForFixedUpdate waitForFixedUpdateEnd = new WaitForFixedUpdate();

    public void ClearActiveController()
    {
        if (ActiveController != null)
        {
            ActiveController.OnFromControllerDisable -= ClearActiveController;
            ActiveController.Disable();
            ActiveController = null;
        }
    }

    public virtual void SwitchToController<T>() where T : ModuleController
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
        ClearActiveController();
        ActiveController = newActiveController;
        ActiveController.Enable();
        ActiveController.OnFromControllerDisable += ClearActiveController;
    }

    protected virtual void Start()
    {
        StartCoroutine(LateFixedUpdate());
    }

    protected virtual void OnDestroy()
    {
        StopAllCoroutines();
    }

    protected virtual void Awake()
    {
        FillControllersList();
    }

    protected virtual void Update()
    {
        if (IsActiveControllerValid())
        {
            ActiveController.RunUpdate();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (IsActiveControllerValid())
        {
            ActiveController.RunFixedUpdate();
        }
    }

    protected virtual void LateUpdate()
    {
        if (IsActiveControllerValid())
        {
            ActiveController.RunLateUpdate();
        }
    }

    protected virtual IEnumerator LateFixedUpdate()
    {
        while (true)
        {
            if (IsActiveControllerValid())
            {
                ActiveController.RunLateFixedUpdate();
            }
            yield return waitForFixedUpdateEnd;
        }
    }

    private bool IsActiveControllerValid()
    {
        return ActiveController != null && ActiveController.enabled && enabled;
    }
    protected abstract void FillControllersList();
}
