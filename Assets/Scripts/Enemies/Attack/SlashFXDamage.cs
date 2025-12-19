using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class SlashFXDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private DamageType damageType = DamageType.Physical;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 0.35f; // ������� ����� ���� (���)

    private CharacterStatsMono owner;                 // ��� �������
    private readonly HashSet<CharacterStatsMono> _hit = new(); // ����� �� ���� ����� ���

    private void Awake()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // ��� �������� ���� ����� ����� Instantiate
    public void Init(CharacterStatsMono ownerStats, float damage, DamageType type)
    {
        owner = ownerStats;
        baseDamage = damage;
        damageType = type;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var receiver = other.GetComponentInParent<IDamageReceiver>();
        if (receiver == null) return;

        var targetStats = other.GetComponentInParent<CharacterStatsMono>();
        if (targetStats == null) return;

        if (owner != null && targetStats == owner) return;
        if (_hit.Contains(targetStats)) return;

        _hit.Add(targetStats);

        float dmg = DamageCalculator.ComputeHit(
            owner,
            targetStats,
            targetStats,
            baseDamage,
            damageType
        );

        Debug.Log($"SlashFX hit target for {dmg}");

        receiver.TakeHit(dmg);
    }
}
