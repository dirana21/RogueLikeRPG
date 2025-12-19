using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class EnemyAttack : MonoBehaviour
{
    [SerializeField] private CharacterStatsMono owner;
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private DamageType damageType = DamageType.Physical;

    private Collider2D hitbox;

    private void Awake()
    {
        hitbox = GetComponent<Collider2D>();
        hitbox.isTrigger = true;
        hitbox.enabled = false;

        if (owner == null)
            owner = GetComponentInParent<CharacterStatsMono>();
    }

    public void EnableHitbox() => hitbox.enabled = true;
    public void DisableHitbox() => hitbox.enabled = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hitbox.enabled) return;

        var receiver = other.GetComponentInParent<IDamageReceiver>();
        if (receiver == null) return;

        var targetStats = other.GetComponentInParent<CharacterStatsMono>();
        if (targetStats == null) return;

        float damage = DamageCalculator.ComputeHit(
            owner,
            targetStats,
            targetStats,
            baseDamage,
            damageType
        );

        Debug.Log($"Enemy hit target for {damage}");

        receiver.TakeHit(damage);

        hitbox.enabled = false;
    }
}