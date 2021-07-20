using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform PlayerTransform => playerTransform;
    public Rigidbody2D PlayerRigidbody2D => rigidbody2D;
    public InputValuesSO InputValues => inputValuesSO;
    public MovementControllersManager MovementControllersManager => movementControllersManager;
    public AimControllersManager AimControllersManager => aimControllersManager;
    public MapGenerator MapGenerator => mapGenerator;
    
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private InputValuesSO inputValuesSO;
    [SerializeField] private MovementControllersManager movementControllersManager;
    [SerializeField] private AimControllersManager aimControllersManager;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private Vector2 velocity;

    private void Start()
    {
        aimControllersManager.SwitchToController<WallAimController>();
        movementControllersManager.SwitchToController<ForwardMovementController>();
    }

    private void Update()
    {
        if (SwitchToGrapMovementAction())
        {
            Debug.Log("SwitchToGrapMovementAction");
            movementControllersManager.SwitchToController<OrbitingMovementController>();
            aimControllersManager.ActiveController.Disable();
        }
        else if(SwitchToForwardMovementAction())
        {
            Debug.Log("SwitchToForwardMovementAction");

            movementControllersManager.SwitchToController<ForwardMovementController>();
            aimControllersManager.ActiveController.Enable();
        }
        else if (UseAbilityAction())
        {
            Debug.Log("Ability");
        }
    }

    private bool UseAbilityAction()
    {
        return movementControllersManager.ActiveController is ForwardMovementController  
            && InputValues.MainAction
            && InputValues.AimDelta == Vector2.zero;
    }

    private bool SwitchToGrapMovementAction()
    {
        return movementControllersManager.ActiveController is ForwardMovementController
            && aimControllersManager.ActiveController != null 
            && aimControllersManager.ActiveController.HasTarget
            && InputValues.MainAction.WasPressed
            && InputValues.AimDelta != Vector2.zero;
    }

    private bool SwitchToForwardMovementAction()
    {
        return movementControllersManager.ActiveController is OrbitingMovementController
            && InputValues.MainAction.WasPressed;
    }

}
