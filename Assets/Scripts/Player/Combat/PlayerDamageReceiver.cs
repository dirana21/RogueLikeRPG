using System;
using UnityEngine;

[RequireComponent(typeof(CharacterStatsMono))]
public sealed class PlayerDamageReceiver : MonoBehaviour, IDamageReceiver, IDamageReceiverEx
{
    // =========================
    // EVENTS
    // =========================

    /// <summary>
    /// Вызывается при получении урона (НЕ смертельного)
    /// </summary>
    public event Action<DamageResult> OnDamageTaken;

    /// <summary>
    /// Вызывается один раз при смерти
    /// </summary>
    public event Action<DamageResult> OnDeath;

    // =========================
    // CONFIG
    // =========================

    [SerializeField] private float invulnerabilityDuration = 0.4f;

    // =========================
    // STATE
    // =========================

    private CharacterStatsMono _stats;
    private bool _isInvulnerable;
    private float _invulnTimer;
    private bool _isDead;

    // =========================
    // UNITY
    // =========================

    private void Awake()
    {
        _stats = GetComponent<CharacterStatsMono>();
        _stats.Model.Health.OnDepleted += HandleDeath;
    }

    private void OnDestroy()
    {
        if (_stats != null)
            _stats.Model.Health.OnDepleted -= HandleDeath;
    }

    private void Update()
    {
        if (!_isInvulnerable)
            return;

        _invulnTimer -= Time.deltaTime;
        if (_invulnTimer <= 0f)
            _isInvulnerable = false;
    }

    // =========================
    // DAMAGE ENTRY POINT
    // =========================

    /// <summary>
    /// Старый контракт — НЕ ЛОМАЕМ
    /// </summary>
    public void TakeHit(float damage)
    {
        if (_isDead || _isInvulnerable)
            return;

        var result = new DamageResult(
            damage: damage,
            evaded: false,
            crit: false,
            fatal: false
        );

        TakeHit(result);
    }

    public void TakeHit(DamageResult result)
    {
        if (_isDead)
            return;

        // ===== INVULNERABILITY: БЕЗ УРОНА, НО С СОБЫТИЕМ =====
        if (_isInvulnerable)
        {
            var blockedResult = new DamageResult(
                damage: 0f,
                evaded: result.Evaded,
                crit: result.Crit,
                fatal: false
            );

            Debug.Log("[PlayerDamageReceiver] Invuln hit -> event fired");
            OnDamageTaken?.Invoke(blockedResult);
            return;
        }

        // ===== APPLY DAMAGE =====
        _stats.Model.Health.Add(-result.Damage);

        bool fatalByHp = _stats.Model.Health.Current <= 0f;
        bool fatal = result.Fatal || fatalByHp;

        var finalResult = new DamageResult(
            damage: result.Damage,
            evaded: result.Evaded,
            crit: result.Crit,
            fatal: fatal
        );

        if (fatal)
        {
            _isDead = true;
            Debug.Log("[PlayerDamageReceiver] Fatal hit");
            OnDeath?.Invoke(finalResult);
            return;
        }

        _isInvulnerable = true;
        _invulnTimer = invulnerabilityDuration;

        Debug.Log("[PlayerDamageReceiver] Normal hit -> event fired");
        OnDamageTaken?.Invoke(finalResult);
    }
    // =========================
    // INTERNAL LOGIC
    // =========================
    private void HandleDeath()
    {
        // Защита от двойного вызова
        if (_isDead)
            return;

        _isDead = true;

        var result = new DamageResult(
            damage: 0f,
            evaded: false,
            crit: false,
            fatal: true
        );

        OnDeath?.Invoke(result);
    }
}
