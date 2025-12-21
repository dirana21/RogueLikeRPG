using UnityEngine;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
    [Header("Refs не трогати, воно шукає шлях само")]
    [SerializeField] private MonoBehaviour inputProvider;
    [SerializeField] private MonoBehaviour moverProvider;
    [SerializeField] private MonoBehaviour facingProvider;
    [SerializeField] private AnimationDriver anim;
    [Header("Tuning")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float staminaCostPerAttack = 15f;

    [SerializeField] private CharacterStatsMono stats;
    private bool _isAttacking;

    private IPlayerInput _input;
    private ICombatInput _combat;
    private IMover _mover;
    private IFacing _facing;

    void Awake()
    {
        _input  = (inputProvider  as IPlayerInput)  ?? GetComponent<IPlayerInput>();
        _combat = (inputProvider  as ICombatInput)  ?? GetComponent<ICombatInput>();
        _mover  = (moverProvider  as IMover)        ?? GetComponent<IMover>();
        _facing = (facingProvider as IFacing)       ?? GetComponent<IFacing>();
        if (anim == null) anim = GetComponent<AnimationDriver>();
        if (stats == null)
            stats = GetComponent<CharacterStatsMono>();
    }

    void Update()
    {
        
        Vector2 dir = new Vector2(_input.Horizontal, _input.Vertical);
        _mover.Move(dir, moveSpeed);
        
        _facing.UpdateFacing(_input.Horizontal);
        
        anim.SetMoveSpeed(_mover.CurrentSpeed);
        
        if (_combat != null && _combat.AttackPressed)
            TryAttack();
        
    }
    public void OnAttackAnimationEnd()
    {
        _isAttacking = false;
    }
    private void TryAttack()
    {
        if (_isAttacking)
            return;

        if (stats == null)
            return;

        if (stats.Model.Stamina.Current < staminaCostPerAttack)
            return;

        StartAttack();
    }
    private void StartAttack()
    {
        _isAttacking = true;

        stats.Model.Stamina.Add(-staminaCostPerAttack);

        anim.PlayAttack();
    }
}