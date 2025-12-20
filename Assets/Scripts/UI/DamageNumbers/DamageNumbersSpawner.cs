using UnityEngine;

public sealed class DamageNumbersSpawner : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private Camera worldCamera;
    [SerializeField] private Transform uiRoot;          // Canvas transform
    [SerializeField] private DamagePopup popupPrefab;

    [Header("Tuning")]
    [SerializeField] private Vector3 worldOffset = new Vector3(0f, 0.6f, 0f);

    private void Awake()
    {
        if (worldCamera == null)
            worldCamera = Camera.main;
    }

    public void Spawn(DamageResult result, Vector3 worldPosition)
    {
        if (popupPrefab == null || uiRoot == null || worldCamera == null)
            return;

        // Evade можно показывать отдельным текстом
        if (result.Evaded)
        {
            SpawnText("EVADE", worldPosition);
            return;
        }

        // Если урон 0 — можно не показывать
        if (result.Damage <= 0f)
            return;

        string msg = Mathf.CeilToInt(result.Damage).ToString();
        if (result.Crit)
            msg = msg + "!";
        
        SpawnText(msg, worldPosition);
    }

    private void SpawnText(string message, Vector3 worldPosition)
    {
        Vector3 screenPos = worldCamera.WorldToScreenPoint(worldPosition + worldOffset);
        var popup = Instantiate(popupPrefab, uiRoot);
        popup.transform.position = screenPos;
        popup.Init(message);
    }
}