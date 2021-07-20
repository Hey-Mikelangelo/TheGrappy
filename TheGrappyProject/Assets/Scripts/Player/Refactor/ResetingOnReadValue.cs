public class ResetingOnReadValue<T> where T : struct
{
    private T value;
    private T defaultValue;
    public ResetingOnReadValue(T defaultValue)
    {
        this.defaultValue = defaultValue;
        value = defaultValue;
    }
    public void Set(T value)
    {
        this.value = value;
    }

    public T Get()
    {
        T valueToReturn = value;
        value = defaultValue;
        return valueToReturn;
    }
}


