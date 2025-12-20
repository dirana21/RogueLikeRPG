using UnityEngine;
using System.Collections;

public sealed class HitStop : MonoBehaviour
{
    public static HitStop Instance { get; private set; }

    [SerializeField] private float defaultDuration = 0.06f;
    [SerializeField] private float defaultTimeScale = 0.05f;

    private Coroutine _routine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play(float duration = -1f, float timeScale = -1f)
    {
        if (_routine != null)
            StopCoroutine(_routine);

        _routine = StartCoroutine(DoHitStop(
            duration > 0 ? duration : defaultDuration,
            timeScale > 0 ? timeScale : defaultTimeScale
        ));
    }

    private IEnumerator DoHitStop(float duration, float scale)
    {
        float originalScale = Time.timeScale;
        Time.timeScale = scale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = originalScale;
    }
}