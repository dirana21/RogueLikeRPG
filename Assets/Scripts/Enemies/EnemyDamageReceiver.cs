using UnityEngine;
using Game.Shared.Stats;

[RequireComponent(typeof(CharacterStatsMono))]

public sealed class EnemyDamageReceiver : MonoBehaviour, IDamageReceiver, IDamageReceiverEx
{
    public event System.Action<DamageResult> OnDamageTaken;
    public event System.Action<DamageResult> OnDied;
    private CharacterStatsMono statsMono;
    private Animator anim;

    private void Awake()
    {
        statsMono = GetComponent<CharacterStatsMono>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        
        if (statsMono?.Model?.Health == null)
        {
            Debug.LogError($"{name}: Model/Health not ready on Start");
            enabled = false;
            return;
        }

        statsMono.Model.Health.OnDepleted += HandleDeath;
    }

    private void OnDestroy()
    {
        if (statsMono?.Model?.Health != null)
            statsMono.Model.Health.OnDepleted -= HandleDeath;
    }

    public void TakeHit(float damage)
    {
        // legacy-вход → переводим в новый формат
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
        if (statsMono?.Model?.Health == null)
            return;

        float before = statsMono.Model.Health.Current;
        statsMono.Model.Health.Add(-result.Damage);
        float after = statsMono.Model.Health.Current;

        Debug.Log($"{name} TAKE HIT {result.Damage:0.##} ({(result.Crit ? "CRIT" : "HIT")}) HP: {before:0.##} -> {after:0.##}");

        if (after <= 0f)
        {
            OnDied?.Invoke(result);
        }
        else
        {
            OnDamageTaken?.Invoke(result);
        }
    }


    private void HandleDeath()
    {
        Debug.Log($"{name} умер");

        var result = new DamageResult(
            damage: 0f,
            evaded: false,
            crit: false,
            fatal: true
        );

        OnDied?.Invoke(result);
        Destroy(gameObject, 0.01f);
    }
}
