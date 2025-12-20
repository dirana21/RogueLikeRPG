using UnityEngine;

[RequireComponent(typeof(Animator))]
public sealed class AnimationDriver : MonoBehaviour
{
    private Animator _anim;

    private enum AnimState
    {
        Idle = 0,
        Move = 10,
        Attack = 80,
        TakeDamage = 60,
        Death = 100
    }

    private AnimState _currentState = AnimState.Idle;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int TakeDamageHash = Animator.StringToHash("TakeDamage");
    private static readonly int IsDeadHash = Animator.StringToHash("IsDead");

    private void Awake()
    {
        _anim = GetComponent<Animator>();

        // Подписка на урон (если есть)
        var damageReceiver = GetComponent<PlayerDamageReceiver>();
        if (damageReceiver != null)
        {
            damageReceiver.OnDamageTaken += HandleTakeDamage;
            damageReceiver.OnDeath += HandleDeath;
        }
    }

    private void OnDestroy()
    {
        var damageReceiver = GetComponent<PlayerDamageReceiver>();
        if (damageReceiver != null)
        {
            damageReceiver.OnDamageTaken -= HandleTakeDamage;
            damageReceiver.OnDeath -= HandleDeath;
        }
    }

    // =========================
    // PUBLIC API (НЕ ЛОМАЕМ)
    // =========================

    public void SetMoveSpeed(float speed)
    {
        if (_currentState >= AnimState.TakeDamage)
            return;

        _anim.SetFloat(SpeedHash, speed);
        _currentState = speed > 0.01f ? AnimState.Move : AnimState.Idle;
    }

    public void PlayAttack()
    {
        TryPlayState(AnimState.Attack, () =>
        {
            _anim.SetTrigger(AttackHash);
        });
    }

    public void PlayTakeDamage()
    {
        TryPlayState(AnimState.TakeDamage, () =>
        {
            _anim.SetTrigger(TakeDamageHash);
        });
    }

    public void PlayDeath()
    {
        if (_currentState == AnimState.Death)
            return;

        _currentState = AnimState.Death;
        _anim.SetBool(IsDeadHash, true);
    }

    // =========================
    // INTERNAL LOGIC
    // =========================

    private void HandleTakeDamage(DamageResult result)
    {
        PlayTakeDamage();
    }

    private void HandleDeath(DamageResult result)
    {
        PlayDeath();
    }

    private void TryPlayState(AnimState newState, System.Action playAction)
    {
        if (newState < _currentState)
            return;

        _currentState = newState;
        playAction.Invoke();
    }

    // Вызывается из Animation Event в конце TakeDamage
    public void ResetToIdle()
    {
        if (_currentState == AnimState.Death)
            return;

        _currentState = AnimState.Idle;
    }
}
