using System;

public class ActionProperty<T>
{
    private T value;

    public T Value
    {
        get => value;
        set
        {
            if (!this.value.Equals(value))
            {
                return;
            }
        
            this.value = value;
            onChangeValue?.Invoke(this.value);
        }
    }
    
    private Action<T> onChangeValue;
    
    public ActionProperty(Action<T> onChangeValue)
    {
        this.onChangeValue = onChangeValue;
    }
}
