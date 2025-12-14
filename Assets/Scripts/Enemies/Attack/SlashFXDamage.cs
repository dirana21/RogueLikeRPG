using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class SlashFXDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private DamageType damageType = DamageType.Physical;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 0.35f; // сколько живет слеш (сек)

    private CharacterStatsMono owner;                 // кто атакует
    private readonly HashSet<CharacterStatsMono> _hit = new(); // чтобы не бить много раз

    private void Awake()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // ЭТО ВЫЗЫВАЕТ ВРАГ СРАЗУ ПОСЛЕ Instantiate
    public void Init(CharacterStatsMono ownerStats, float damage, DamageType type)
    {
        owner = ownerStats;
        baseDamage = damage;
        damageType = type;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.GetComponent<CharacterStatsMono>();
        if (target == null) return;
        if (owner != null && target == owner) return;

        // чтобы не наносить урон одной цели много раз (коллайдер может сработать несколько раз)
        if (_hit.Contains(target)) return;
        _hit.Add(target);

        float dmg = DamageCalculator.ComputeHit(
            owner,   // attacker
            target,  // target stats
            target,  // target resists
            baseDamage,
            damageType
        );

        target.Model.Health.Add(-dmg);
    }
}
