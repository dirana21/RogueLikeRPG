using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBarsUI : MonoBehaviour
{
    [SerializeField] private CharacterStatsMono stats;

    [Header("HP")]
    [SerializeField] private Image hpFill;
    [SerializeField] private TMP_Text hpText;

    [Header("Stamina")]
    [SerializeField] private Image staminaFill;
    [SerializeField] private TMP_Text staminaText;

    private void Start()
    {
        if (!stats)
            stats = FindObjectOfType<CharacterStatsMono>();
        
        stats.Model.Health.OnChanged += OnHealthChanged;
        stats.Model.Stamina.OnChanged += OnStaminaChanged;

        RefreshAll();
    }

    private void OnDestroy()
    {
        if (stats == null) return;

        stats.Model.Health.OnChanged -= OnHealthChanged;
        stats.Model.Stamina.OnChanged -= OnStaminaChanged;
    }

    private void OnHealthChanged(IVital hp)
    {
        float percent = hp.Current / hp.Max;
        hpFill.fillAmount = percent;

        if (hpText)
            hpText.text = $"{Mathf.CeilToInt(hp.Current)} / {Mathf.CeilToInt(hp.Max)}";
    }

    private void OnStaminaChanged(IVital stamina)
    {
        float percent = stamina.Current / stamina.Max;
        staminaFill.fillAmount = percent;

        if (staminaText)
            staminaText.text = $"{Mathf.CeilToInt(stamina.Current)} / {Mathf.CeilToInt(stamina.Max)}";
    }

    private void RefreshAll()
    {
        OnHealthChanged(stats.Model.Health);
        OnStaminaChanged(stats.Model.Stamina);
    }
}