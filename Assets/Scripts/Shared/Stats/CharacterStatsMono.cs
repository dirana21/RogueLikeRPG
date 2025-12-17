using Game.Shared.Stats;
using UnityEngine;

public sealed class CharacterStatsMono : MonoBehaviour, IStatsProvider, IResistanceProvider
{
    [SerializeField] private BaseStatsConfig baseConfig;

    public CharacterStats Model { get; private set; }

    void Awake()
    {
        var calc = new DefaultStatCalculator();
        Model = new CharacterStats(calc, baseConfig);
        Model.Health.OnDepleted += OnDeath;
    }

    void Update() => Model.Tick(Time.deltaTime);

    public float Get(string id) => Model.Get(id);
    public float GetResistance(DamageType type) => Model.GetResistance(type);

    private void OnDeath()
    {
        // anim.SetBool("isDead", true);
    }
}