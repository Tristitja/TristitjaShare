namespace Tristitja.Share;

public class CounterState
{
    public int Value
    {
        get => field;
        set
        {
            field = value;
            OnChange?.Invoke(field);
        }
    }

    public event Action<int>? OnChange;
}
