public sealed class Stat : IStat
{
    public string Id { get; }
    public float Base { get; private set; }
    public float Value { get; private set; } 

    public Stat(string id, float @base)
    {
        Id = id; Base = @base; Value = @base;
    }
    public void SetBase(float value) => Base = value;
    public void SetComputed(float value) => Value = value; 
}