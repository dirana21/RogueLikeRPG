using UnityEngine;
using TMPro;
using Game.Shared.Stats;

public class StatsPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [Header("Text fields")]
    [SerializeField] private TMP_Text titleText;          // TitleTXT (Stats)
    [SerializeField] private TMP_Text hpText;             // HPText
    [SerializeField] private TMP_Text staminaText;        // StaminaText
    [SerializeField] private TMP_Text baseStatsTitle;     // BaseStatsTitle
    [SerializeField] private TMP_Text baseStatsText;      // BaseStatsText
    [SerializeField] private TMP_Text resTitle;           // ResistancesTitle
    [SerializeField] private TMP_Text resText;            // ResistancesText

    [SerializeField] private CharacterStatsMono statsMono;

    private bool isOpen = false;

    void Start()
    {
        if (statsMono != null)
            statsMono.Model.OnStatsChanged += Refresh;

        if (titleText != null)
            titleText.text = "<b>Player stats</b>";
        if (baseStatsTitle != null)
            baseStatsTitle.text = "<b>Stats</b>";
        if (resTitle != null)
            resTitle.text = "<b>Resistance</b>";
    }

    void OnDestroy()
    {
        if (statsMono != null)
            statsMono.Model.OnStatsChanged -= Refresh;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            TogglePanel();
    }

    private void TogglePanel()
    {
        isOpen = !isOpen;
        panel.SetActive(isOpen);

        if (isOpen)
            Refresh();
    }

    public void Refresh()
    {
        if (statsMono == null) return;
        var m = statsMono.Model;

        if (hpText != null)
            hpText.text = $"<b><color=#FF7070>HP:</color></b> {m.Health.Current} / {m.Health.Max}";

        if (staminaText != null)
            staminaText.text = $"<b><color=#70C3FF>Stamina:</color></b> {m.Stamina.Current} / {m.Stamina.Max}";

        if (baseStatsText != null)
            baseStatsText.text = BuildBaseStats(m);

        if (resText != null)
            resText.text = BuildResists(m);
    }

    private string BuildBaseStats(CharacterStats m)
    {
        var sb = new System.Text.StringBuilder();

        foreach (var kv in m.Stats)
        {
            string name = kv.Key.Replace("_", " ");
            sb.AppendLine($"{name}: {kv.Value.Value}");
        }

        return sb.ToString();
    }

    private string BuildResists(CharacterStats m)
    {
        var sb = new System.Text.StringBuilder();

        foreach (DamageType dt in System.Enum.GetValues(typeof(DamageType)))
        {
            float val = m.GetResistance(dt);
            sb.AppendLine($"{dt}: {val}");
        }

        return sb.ToString();
    }
}
