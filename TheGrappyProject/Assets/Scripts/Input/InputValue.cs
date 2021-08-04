public abstract class InputValue<T>
{
    public T Value { get; internal set; }
    public bool WasReleased { get; internal set; }

    public static implicit operator T(InputValue<T> inputValue)
    {
        return inputValue.Value;
    }
}


