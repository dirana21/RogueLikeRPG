using UnityEngine;

public static class DamageCalculator
{
    private static readonly System.Random _rng = new();

    public static float ComputeHit(CharacterStats target, CharacterStats attacker, float baseDamage, DamageType type)
    {
        // Уклонение цели
        if (_rng.NextDouble() < target.Get(CharacterStats.Evasion))
            return 0f;

        // Крит атакера
        float damage = baseDamage;
        if (_rng.NextDouble() < attacker.Get(CharacterStats.CritChance))
            damage *= attacker.Get(CharacterStats.CritDamage);

        // Резист цели
        float resist = target.GetResistance(type); // 0..1
        damage *= (1f - resist);

        // Влияние силы/интеллекта — по вашей формуле (пример)
        damage += attacker.Get(CharacterStats.Strength) * 0.5f;

        return Mathf.Max(0f, damage);
    }
}