using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class EnemyHealthBarUI : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private CharacterStatsMono target;
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text hpText;

    private void Reset()
    {
        fill = GetComponentInChildren<Image>(true);
        hpText = GetComponentInChildren<TMP_Text>(true);
    }

    private void LateUpdate()
    {
        if (target == null || target.Model == null || target.Model.Health == null)
            return;

        float max = target.Model.Health.Max;
        float cur = target.Model.Health.Current;

        if (max <= 0.0001f) return;

        float t = Mathf.Clamp01(cur / max);

        // Полоска
        if (fill != null)
            fill.fillAmount = t;

        // Текст
        if (hpText != null)
            hpText.text = $"{Mathf.CeilToInt(cur)}/{Mathf.CeilToInt(max)}";
    }

    // чтобы быстро проверить руками из инспектора
    public void SetTarget(CharacterStatsMono newTarget) => target = newTarget;
}
