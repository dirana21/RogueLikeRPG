using UnityEngine;

public static class DamageCalculator
{
    private static readonly System.Random _rng = new();

    public static float ComputeHit(
        IStatsProvider attacker,
        IStatsProvider targetStats,
        IResistanceProvider targetResists,
        float baseDamage,
        DamageType type)
    {
        // Уклонение цели
        if (_rng.NextDouble() < targetStats.Get(CharacterStats.Evasion))
            return 0f;

        float damage = baseDamage;

        // Крит атакера
        if (_rng.NextDouble() < attacker.Get(CharacterStats.CritChance))
            damage *= attacker.Get(CharacterStats.CritDamage);

        // Резист цели
        float resist = targetResists.GetResistance(type); // 0..1
        damage *= (1f - resist);

        // Сила/интеллект атакующего
        damage += attacker.Get(CharacterStats.Strength) * 0.5f;

        // Модификатор получаемого урона цели
        damage *= targetStats.Get(CharacterStats.DamageTakenMult);

        return Mathf.Max(0f, damage);
    }
}