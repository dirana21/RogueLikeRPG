public enum ModifierOp { Add, Mul } 

public interface IStatModifier
{
    string StatId { get; }         
    ModifierOp Op { get; }
    float Amount { get; }          
    int Order { get; }             
    bool IsExpired(float time);    
}