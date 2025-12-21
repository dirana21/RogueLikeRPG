using UnityEngine;
using System.Collections;

public sealed class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [SerializeField] private float defaultIntensity = 0.15f;
    [SerializeField] private float defaultDuration = 0.12f;

    private Vector3 _originalPos;
    private Coroutine _routine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _originalPos = transform.localPosition;
    }

    public void Shake(float intensity = -1f, float duration = -1f)
    {
        if (_routine != null)
            StopCoroutine(_routine);

        _routine = StartCoroutine(DoShake(
            intensity > 0 ? intensity : defaultIntensity,
            duration > 0 ? duration : defaultDuration
        ));
    }

    private IEnumerator DoShake(float intensity, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            transform.localPosition =
                _originalPos +
                (Vector3)Random.insideUnitCircle * intensity;

            t += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localPosition = _originalPos;
    }
}