public sealed class Vital : IVital
{
    public string Id { get; }
    public float Max { get; private set; }
    public float Current { get; private set; }

    public event System.Action<IVital> OnChanged;
    public event System.Action OnDepleted;

    public Vital(string id, float max)
    {
        Id = id; Max = max; Current = max;
    }

    public void SetMax(float value)
    {
        Max = value; if (Current > Max) Current = Max; OnChanged?.Invoke(this);
    }

    public void SetCurrent(float value)
    {
        Current = UnityEngine.Mathf.Clamp(value, 0, Max); OnChanged?.Invoke(this); if (Current <= 0) OnDepleted?.Invoke();
    }
    public void Add(float delta) => SetCurrent(Current + delta);
}