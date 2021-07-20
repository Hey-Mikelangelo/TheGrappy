using UnityEngine;

public abstract class PlayerModuleController : MonoBehaviour
{
    [SerializeField] protected Player player;
    protected bool isEnabled;

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public virtual void Enable()
    {
        isEnabled = true;
    }

    public virtual void Disable()
    {
        isEnabled = false;
    }
    public abstract void RunUpdate();
    public virtual void RunFixedUpdate() { }
}


