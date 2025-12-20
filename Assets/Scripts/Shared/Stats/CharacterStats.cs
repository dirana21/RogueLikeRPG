using System.Collections.Generic;
using Game.Shared.Stats;
using UnityEngine;

public sealed class CharacterStats : IStatsProvider, IResistanceProvider
{
    public const string Agility = "agility";
    public const string Strength = "strength";
    public const string Intellect = "intellect";
    public const string SwordRadius = "sword_radius";
    public const string Knockback = "knockback";
    public const string CritChance = "crit_chance";     
    public const string CritDamage = "crit_damage";     
    public const string StaminaDamage = "stamina_damage";
    public const string AttackSpeed = "attack_speed";   
    public const string Evasion = "evasion"; 
    public const string DamageTakenMult = "damage_taken_mult";
    public const string HealthMax = "health_max";
    public const string staminaRegenRate = "stamina_regen_rate";
    
    public Vital Health { get; }
    public Vital Stamina { get; }

    public event System.Action OnStatsChanged;
    
    private readonly Dictionary<string, Stat> _stats = new();
    private readonly List<IStatModifier> _mods = new();
    private readonly IStatCalculator _calc;
    private readonly Dictionary<DamageType, Stat> _resists = new();
    private float _lastHpMax = -1f;
    public bool keepHpRatioOnMaxChange = true;
    
    

    private float _time;

    public CharacterStats(IStatCalculator calc, BaseStatsConfig cfg)
    {
        _calc = calc;

        
        AddStat(Agility,        cfg.agility);
        AddStat(Strength,       cfg.strength);
        AddStat(Intellect,      cfg.intellect);
        AddStat(SwordRadius,    cfg.swordRadius);
        AddStat(Knockback,      cfg.knockback);
        AddStat(CritChance,     cfg.critChance);
        AddStat(CritDamage,     cfg.critDamage);
        AddStat(StaminaDamage,  cfg.staminaDamage);
        AddStat(AttackSpeed,    cfg.attackSpeed);
        AddStat(Evasion,        cfg.evasion);
        AddStat(DamageTakenMult, cfg.damageTakenMultiplier);
        AddStat(HealthMax, cfg.maxHealth);
        AddStat(staminaRegenRate, cfg.staminaRegen);

        
        Health  = new Vital("health",  cfg.maxHealth);
        Stamina = new Vital("stamina", cfg.maxStamina);

        
        _resists[DamageType.Fire]      = new Stat("res_fire",      cfg.resFire);
        _resists[DamageType.Cold]      = new Stat("res_cold",      cfg.resCold);
        _resists[DamageType.Darkness]  = new Stat("res_dark",      cfg.resDark);
        _resists[DamageType.Poison]    = new Stat("res_poison",    cfg.resPoison);
        _resists[DamageType.Corrosion] = new Stat("res_corrosion", cfg.resCorrosion);
        _resists[DamageType.Lightning] = new Stat("res_lightning", cfg.resLightning);

        Recompute();
    }

    private void AddStat(string id, float baseValue) => _stats[id] = new Stat(id, baseValue);

    public void Tick(float dt)
    {
        _time += dt;
        _mods.RemoveAll(m => m.IsExpired(_time));
        RegenerateStamina(dt);
        Recompute();
    }

    public void AddModifier(IStatModifier mod) { _mods.Add(mod); Recompute(); }
    public void RemoveModifier(IStatModifier mod)
    {
        if (_mods.Remove(mod))
            Recompute();
    }

    public float Get(string id) => _stats.TryGetValue(id, out var s) ? s.Value : 0f;

    public float GetResistance(DamageType type)
    {
        if (_resists.TryGetValue(type, out var s)) return Mathf.Clamp01(s.Value);
        return 0f;
    }

    public void Recompute()
    {
        _calc.RecomputeAll(_stats, _mods);
        _calc.RecomputeAll(_resists, _mods);

        if (_stats.TryGetValue(HealthMax, out var hpMaxStat))
        {
            float oldMax = (_lastHpMax < 0f) ? Health.Max : _lastHpMax;
            float newMax = hpMaxStat.Value;

            if (keepHpRatioOnMaxChange && oldMax > 0f)
            {
                float ratio = Health.Current / oldMax;
                Health.SetMax(newMax);
                Health.SetCurrent(Mathf.Clamp01(ratio) * newMax);
            }
            else
            {
                Health.SetMax(newMax);
                Health.SetCurrent(Mathf.Min(Health.Current, newMax));
            }

            _lastHpMax = newMax;
        }
        OnStatsChanged?.Invoke();
    }
    public IReadOnlyDictionary<string, Stat> Stats => _stats;
    public IReadOnlyDictionary<DamageType, Stat> Resists => _resists;
    
    public bool TryRemoveModifier(IStatModifier mod)
    {
        bool removed = _mods.Remove(mod);
        if (removed) Recompute();
        
        return removed;
    }
    
    public int RemoveModifiers(IEnumerable<IStatModifier> mods)
    {
        int removed = 0;
        foreach (var m in mods)
            if (_mods.Remove(m)) removed++;
        if (removed > 0) Recompute();
        return removed;
    }
    private void RegenerateStamina(float dt)
    {
        float regen = Get(staminaRegenRate);
        if (regen <= 0f)
            return;

        if (Stamina.Current < Stamina.Max)
            Stamina.Add(regen * dt);
    }
    public bool ApplyDamage(float damage)
    {
        Health.Add(-damage);
        return Health.Current <= 0f;
    }
}