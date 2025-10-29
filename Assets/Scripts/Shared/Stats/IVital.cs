public interface IVital
{
    string Id { get; }        
    float Max { get; }
    float Current { get; }
    event System.Action<IVital> OnChanged;
    event System.Action OnDepleted; 
    void SetMax(float value);
    void SetCurrent(float value);
    void Add(float delta);
}