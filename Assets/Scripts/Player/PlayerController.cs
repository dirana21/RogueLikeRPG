using UnityEngine;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
    [Header("Refs не трогать, оно ищет путь само")]
    [SerializeField] private MonoBehaviour inputProvider;   // IPlayerInput
    [SerializeField] private MonoBehaviour moverProvider;   // IMover
    [SerializeField] private MonoBehaviour facingProvider;  // IFacing
    [SerializeField] private AnimationDriver anim;
    [Header("Tuning")]
    [SerializeField] private float moveSpeed = 4f;

    private IPlayerInput _input;
    private ICombatInput _combat;
    private IMover _mover;
    private IFacing _facing;

    void Awake()
    {
        // внедрение зависимостей (DIP)
        _input  = (inputProvider  as IPlayerInput)  ?? GetComponent<IPlayerInput>();
        _combat = (inputProvider  as ICombatInput)  ?? GetComponent<ICombatInput>();
        _mover  = (moverProvider  as IMover)        ?? GetComponent<IMover>();
        _facing = (facingProvider as IFacing)       ?? GetComponent<IFacing>();
        if (anim == null) anim = GetComponent<AnimationDriver>();
    }

    void Update()
    {
        // 1) движение
        Vector2 dir = new Vector2(_input.Horizontal, _input.Vertical);
        _mover.Move(dir, moveSpeed);

        // 2) разворот
        _facing.UpdateFacing(_input.Horizontal);

        // 3) анимация бега
        anim.SetMoveSpeed(_mover.CurrentSpeed);

        // 4) атака (пример)
        if (_combat != null && _combat.AttackPressed)
            anim.PlayAttack();
    }
}