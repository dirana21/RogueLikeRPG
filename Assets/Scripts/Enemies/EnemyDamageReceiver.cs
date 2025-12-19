using UnityEngine;
using Game.Shared.Stats;

[RequireComponent(typeof(CharacterStatsMono))]
public sealed class EnemyDamageReceiver : MonoBehaviour, IDamageReceiver
{
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

        statsMono.Model.Health.OnDepleted += OnDeath;
    }

    private void OnDestroy()
    {
        if (statsMono?.Model?.Health != null)
            statsMono.Model.Health.OnDepleted -= OnDeath;
    }

    public void TakeHit(float damage)
    {
        if (statsMono?.Model?.Health == null) return;

        float before = statsMono.Model.Health.Current;
        statsMono.Model.Health.Add(-damage);
        float after = statsMono.Model.Health.Current;

        Debug.Log($"{name} TAKE HIT {damage:0.##}. HP: {before:0.##} -> {after:0.##}");
    }


    private void OnDeath()
    {
        Debug.Log($"{name} умер");
        Destroy(gameObject, 0.01f);
    }
}
