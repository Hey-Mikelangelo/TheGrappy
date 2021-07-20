using UnityEngine;

[CreateAssetMenu(fileName = "InputValuesSO", menuName = "Game/Input/InputValuesSO")]
public class InputValuesSO : ScriptableObject
{
    public Vector2InputValue AimDelta { get; internal set; } = new Vector2InputValue();
    public BoolInputValue MainAction { get; internal set; } = new BoolInputValue();

}


