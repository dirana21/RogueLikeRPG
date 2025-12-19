using UnityEngine;

[RequireComponent(typeof(CharacterStatsMono))]
[RequireComponent(typeof(AnimationDriver))]
public sealed class PlayerDamageReceiver : MonoBehaviour, IDamageReceiver
{
    private CharacterStatsMono stats;
    private AnimationDriver anim;

    private void Awake()
    {
        stats = GetComponent<CharacterStatsMono>();
        anim  = GetComponent<AnimationDriver>();

        
        stats.Model.Health.OnDepleted += OnDeath;
    }

    public void TakeHit(float damage)
    {
        Debug.Log($"PLAYER TAKE HIT: {damage}");

        stats.Model.Health.Add(-damage);

        if (stats.Model.Health.Current > 0)
            anim.PlayTakeDamage();
    }

    private void OnDeath()
    {
        anim.PlayDeath();
    }
}