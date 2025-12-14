public sealed class Vital : IVital
{
    public string Id { get; }
    public float Max { get; private set; }
    public float Current { get; private set; }

    public event System.Action<IVital> OnChanged;
    public event System.Action OnDepleted;

    private bool _depleted; // <-- добавили

    public Vital(string id, float max)
    {
        Id = id;
        Max = max;
        Current = max;
        _depleted = Current <= 0f;
    }

    public void SetMax(float value)
    {
        Max = value;
        if (Current > Max) Current = Max;
        OnChanged?.Invoke(this);
    }

    public void SetCurrent(float value)
    {
        float prev = Current;
        Current = UnityEngine.Mathf.Clamp(value, 0, Max);

        // если мы "воскресли" (например хилом) Ч сбрасываем флаг
        if (Current > 0f) _depleted = false;

        OnChanged?.Invoke(this);

        // OnDepleted должен сработать только при переходе >0 -> 0
        if (!_depleted && prev > 0f && Current <= 0f)
        {
            _depleted = true;
            OnDepleted?.Invoke();
        }
    }

    public void Add(float delta) => SetCurrent(Current + delta);
}
