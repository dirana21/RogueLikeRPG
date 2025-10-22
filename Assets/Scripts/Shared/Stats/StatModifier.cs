public sealed class StatModifier : IStatModifier
{
    public string StatId { get; }
    public ModifierOp Op { get; }
    public float Amount { get; }
    public int Order { get; }
    private readonly float _expireAt; 

    public StatModifier(string statId, ModifierOp op, float amount, int order = 0, float duration = -1f, float now = 0f)
    {
        StatId = statId; Op = op; Amount = amount; Order = order;
        _expireAt = duration > 0 ? now + duration : -1f;
    }
    public bool IsExpired(float time) => _expireAt > 0 && time >= _expireAt;
}