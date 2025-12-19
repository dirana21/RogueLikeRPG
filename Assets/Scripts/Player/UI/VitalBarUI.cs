using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class VitalBarUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text valueText;

    [Header("Settings")]
    [SerializeField] private float lerpSpeed = 10f;
    [SerializeField] private Color normalColor = Color.red;
    [SerializeField] private Color lowColor = Color.yellow;
    [SerializeField] private float lowThreshold = 0.3f;

    private float _targetFill = 1f;

    public void SetValue(float current, float max)
    {
        _targetFill = Mathf.Clamp01(current / max);

        if (valueText)
            valueText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";

        if (fillImage)
            fillImage.color = _targetFill <= lowThreshold ? lowColor : normalColor;
    }

    void Update()
    {
        if (!fillImage) return;

        fillImage.fillAmount = Mathf.Lerp(
            fillImage.fillAmount,
            _targetFill,
            Time.deltaTime * lerpSpeed
        );
    }
}