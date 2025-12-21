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
        
        if (_rng.NextDouble() < targetStats.Get(CharacterStats.Evasion))
            return 0f;

        float damage = baseDamage;

        
        if (_rng.NextDouble() < attacker.Get(CharacterStats.CritChance))
            damage *= attacker.Get(CharacterStats.CritDamage);

        
        float resist = targetResists.GetResistance(type); 
        damage *= (1f - resist);

        
        damage += attacker.Get(CharacterStats.Strength) * 0.5f;

        
        damage *= targetStats.Get(CharacterStats.DamageTakenMult);

        return Mathf.Max(0f, damage);
    }
}