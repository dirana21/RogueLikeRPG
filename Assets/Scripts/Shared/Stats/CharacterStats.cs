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
    
    public Vital Health { get; }
    public Vital Stamina { get; }

    private readonly Dictionary<string, Stat> _stats = new();
    private readonly List<IStatModifier> _mods = new();
    private readonly IStatCalculator _calc;
    
    private readonly Dictionary<DamageType, Stat> _resists = new();

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
        // скинуть протухшие моды
        _mods.RemoveAll(m => m.IsExpired(_time));
        Recompute();
    }

    public void AddModifier(IStatModifier mod) { _mods.Add(mod); Recompute(); }
    public void RemoveModifier(IStatModifier mod) { _mods.Remove(mod); Recompute(); }

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
    }
}