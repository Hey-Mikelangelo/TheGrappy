using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    public Transform PlayerTransform => playerTransform;
    public Rigidbody2D PlayerRigidbody2D => rigidbody2D;
    public InputValuesSO InputValues => inputValuesSO;
    public MovementControllersManager MovementControllersManager => movementControllersManager;
    public AimControllersManager AimControllersManager => aimControllersManager;
    public AbilityControllersManager AbilityControllersManager => abilityControllersManager;
    public MapGenerationManager MapGenerationManager => mapGenerationManager;
    public GameProgressManager GameProgressManager => gameProgressManager;
    public CollectedAbilitiesPocket CollectedAbilitiesPocket => collectedAbilitiesPocket;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private MovementControllersManager movementControllersManager;
    [SerializeField] private AimControllersManager aimControllersManager;
    [SerializeField] private MapGenerationManager mapGenerationManager;
    [SerializeField] private AbilityControllersManager abilityControllersManager;
    [SerializeField] private InputValuesSO inputValuesSO;

    [Inject] private GameProgressManager gameProgressManager;
    [Inject] private CollectedAbilitiesPocket collectedAbilitiesPocket;

    private void Start()
    {
        AimControllersManager.SwitchToController<WallAimController>();
        MovementControllersManager.SwitchToController<ForwardMovementController>();
        this.GameProgressManager.Init();
        this.MapGenerationManager.Init();
    }

    private void OnDestroy()
    {
        this.GameProgressManager.Finish();
        this.MapGenerationManager.Finish();
    }

    private void Update()
    {
        if (SwitchToGrapMovementAction())
        {
            MovementControllersManager.SwitchToController<OrbitingMovementController>();
            AimControllersManager.SwitchToController<SimpleAimController>();
        }
        else if(SwitchToForwardMovementAction())
        {
            MovementControllersManager.SwitchToController<ForwardMovementController>();
            AimControllersManager.SwitchToController<WallAimController>();
        }
        else if (UseAbilityAction() && abilityControllersManager.ActiveController == null)
        {
            abilityControllersManager.SwitchToController<SideBoostController>();
        }
        else if(EndAbilityAction() && abilityControllersManager.ActiveController != null)
        {
            abilityControllersManager.ClearActiveController();
        }

        this.GameProgressManager.UpdateTick();
        this.MapGenerationManager.UpdateTick();

    }

    private bool UseAbilityAction()
    {
        return MovementControllersManager.ActiveController is ForwardMovementController  
            && InputValues.MainAction.WasPressed
            && InputValues.AimDelta == Vector2.zero;
    }

    private bool EndAbilityAction()
    {
        return InputValues.MainAction.WasReleased;
    }

    private bool SwitchToGrapMovementAction()
    {
        return MovementControllersManager.ActiveController is ForwardMovementController
            && AimControllersManager.ActiveController != null 
            && AimControllersManager.ActiveController.HasTarget
            && InputValues.MainAction.WasPressed
            && InputValues.AimDelta != Vector2.zero;
    }

    private bool SwitchToForwardMovementAction()
    {
        return MovementControllersManager.ActiveController is OrbitingMovementController
            && InputValues.MainAction.WasPressed
            && InputValues.AimDelta != Vector2.zero;
    }

}
