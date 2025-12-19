using UnityEngine;

public sealed class PlayerHUDBinder : MonoBehaviour
{
    [SerializeField] private CharacterStatsMono statsMono;
    [SerializeField] private VitalBarUI hpBar;
    [SerializeField] private VitalBarUI staminaBar;

    void Start()
    {
        var model = statsMono.Model;

        model.Health.OnChanged += OnHealthChanged;
        model.Stamina.OnChanged += OnStaminaChanged;

        // первичное заполнение
        OnHealthChanged(model.Health);
        OnStaminaChanged(model.Stamina);
    }

    void OnDestroy()
    {
        if (!statsMono) return;

        statsMono.Model.Health.OnChanged -= OnHealthChanged;
        statsMono.Model.Stamina.OnChanged -= OnStaminaChanged;
    }

    private void OnHealthChanged(IVital v)
    {
        hpBar.SetValue(v.Current, v.Max);
    }

    private void OnStaminaChanged(IVital v)
    {
        staminaBar.SetValue(v.Current, v.Max);
    }
}