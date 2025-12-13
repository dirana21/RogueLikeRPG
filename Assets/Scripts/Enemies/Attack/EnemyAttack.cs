using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class EnemyAttack : MonoBehaviour
{
    [SerializeField] private CharacterStatsMono owner; // враг (родитель)
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

        var target = other.GetComponent<CharacterStatsMono>();
        if (target == null || target == owner) return;

        float damage = DamageCalculator.ComputeHit(
            owner,   // attacker
            target,  // target stats
            target,  // target resists (у тебя CharacterStatsMono это умеет)
            baseDamage,
            damageType
        );

        // урон = отрицательная дельта
        target.Model.Health.Add(-damage);

        // если хочешь "один удар за атаку", можно тут же выключить
        // hitbox.enabled = false;
    }
}
