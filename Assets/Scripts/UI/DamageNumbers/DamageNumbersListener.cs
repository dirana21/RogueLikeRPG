using UnityEngine;

public sealed class DamageNumbersListener : MonoBehaviour
{
    [SerializeField] private DamageNumbersSpawner spawner;

    private void Awake()
    {
        // 1) ищем спавнер на сцене (самый простой способ)
        if (spawner == null)
            spawner = FindFirstObjectByType<DamageNumbersSpawner>();

        // 2) Подписка на игрока
        var player = GetComponent<PlayerDamageReceiver>();
        if (player != null)
        {
            player.OnDamageTaken += OnDamage;
            player.OnDeath += OnDamage;
        }

        // 3) Подписка на врага
        var enemy = GetComponent<EnemyDamageReceiver>();
        if (enemy != null)
        {
            enemy.OnDamageTaken += OnDamage;
            enemy.OnDied += OnDamage;
        }
    }

    private void OnDestroy()
    {
        var player = GetComponent<PlayerDamageReceiver>();
        if (player != null)
        {
            player.OnDamageTaken -= OnDamage;
            player.OnDeath -= OnDamage;
        }

        var enemy = GetComponent<EnemyDamageReceiver>();
        if (enemy != null)
        {
            enemy.OnDamageTaken -= OnDamage;
            enemy.OnDied -= OnDamage;
        }
    }

    private void OnDamage(DamageResult result)
    {
        Debug.Log($"[POPUP LISTENER] {gameObject.name} dmg={result.Damage}");

        if (spawner == null)
        {
            Debug.LogError("[POPUP LISTENER] Spawner is NULL");
            return;
        }

        spawner.Spawn(result, transform.position);
    }
}