public readonly struct DamageResult
{
    public readonly float Damage;
    public readonly bool Evaded;
    public readonly bool Crit;
    public readonly bool Fatal;

    public DamageResult(float damage, bool evaded, bool crit, bool fatal)
    {
        Damage = damage;
        Evaded = evaded;
        Crit = crit;
        Fatal = fatal;
    }

    // Удобные хелперы на будущее (НЕ ОБЯЗАТЕЛЬНЫ, НО ПОЛЕЗНЫ)
    public DamageResult AsFatal()
        => new DamageResult(Damage, Evaded, Crit, true);

    public DamageResult AsCrit()
        => new DamageResult(Damage, Evaded, true, Fatal);

    public static DamageResult Zero()
        => new DamageResult(0f, false, false, false);
}