using UnityEngine;
using Zenject;

public abstract class PlayerModuleController : MonoBehaviour
{
    public event System.Action OnFromControllerDisable;

    [Inject] protected Player player;
    protected bool isEnabled;

    public virtual void Enable()
    {
        isEnabled = true;
    }

    public virtual void Disable()
    {
        isEnabled = false;
    }

    public virtual void RunUpdate() { }
    public virtual void RunFixedUpdate() { }
    public virtual void RunLateUpdate() { }
    public virtual void RunLateFixedUpdate() { }

    protected void DisableThisController()
    {
        OnFromControllerDisable?.Invoke();
    }
}


