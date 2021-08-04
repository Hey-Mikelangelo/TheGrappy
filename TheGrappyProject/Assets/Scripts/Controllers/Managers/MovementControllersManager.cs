using UnityEngine;
using Zenject;

public class MovementControllersManager : PlayerModuleControllerManager<MovementController>
{
    [SerializeField] private AnimationCurve scoreToSpeedCurve;
    
    [SerializeField]
    private ForwardMovementController forwardMovementController;
    [SerializeField]
    private OrbitingMovementController grapMovementController;

    protected override void FillControllersList()
    {
        controllers.Add(grapMovementController);
        controllers.Add(forwardMovementController);
    }

    protected override void Update()
    {
        base.Update();
        float speed = scoreToSpeedCurve.Evaluate(player.GameProgressManager.GetTimeSurvived());
        if(ActiveController != null)
        {
            ActiveController.MovementSpeed = speed;
        }
    }
}
