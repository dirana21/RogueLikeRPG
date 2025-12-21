using TMPro;
using UnityEngine;

public sealed class DamagePopup : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float lifetime = 0.8f;
    [SerializeField] private float floatUpSpeed = 40f;

    private float _t;

    public void Init(string message)
    {
        if (text == null)
            text = GetComponent<TMP_Text>();

        text.text = message;
    }

    private void Update()
    {
        // двигаем вверх
        transform.position += Vector3.up * (floatUpSpeed * Time.unscaledDeltaTime);

        // умираем по таймеру
        _t += Time.unscaledDeltaTime;
        if (_t >= lifetime)
            Destroy(gameObject);
    }
}