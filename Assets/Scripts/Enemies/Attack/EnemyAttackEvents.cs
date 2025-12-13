using UnityEngine;

public sealed class EnemyAttackEvents : MonoBehaviour
{
    [Header("Slash FX")]
    [SerializeField] private GameObject slashFxPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform slashSpawnPointRight;
    [SerializeField] private Transform slashSpawnPointLeft;
    [SerializeField] private Transform slashSpawnPointUp;
    [SerializeField] private Transform slashSpawnPointDown;

    [Header("Damage")]
    [SerializeField] private float slashBaseDamage = 10f;
    [SerializeField] private DamageType slashDamageType = DamageType.Physical;

    [Header("Visual Tuning")]
    [SerializeField] private float slashScale = 1f; // подгони (например 1.3-1.8)
    [SerializeField] private bool rotateUpDown = true; // если слеш должен "поворачиваться" вверх/вниз

    private CharacterStatsMono _owner;
    private EnemyMovement _movement;

    private void Awake()
    {
        _owner = GetComponent<CharacterStatsMono>();
        _movement = GetComponent<EnemyMovement>();
    }

    // Animation Event (в клипе атаки)
    public void SpawnSlashFX()
    {
        if (slashFxPrefab == null) return;
        if (_movement == null) return;

        Vector2 dir = _movement.LastAimDir;
        Transform point = ChooseSpawnPoint(dir);
        if (point == null) point = transform;

        // Спавним
        var go = Instantiate(slashFxPrefab, point.position, Quaternion.identity);

        // 1) Масштаб
        go.transform.localScale = Vector3.one * slashScale;

        // 2) Ориентация (лево/право + вверх/вниз)
        ApplyOrientation(go.transform, dir);

        // 3) Инициализация урона
        var fx = go.GetComponent<SlashFXDamage>();
        if (fx != null)
            fx.Init(_owner, slashBaseDamage, slashDamageType);
    }

    private Transform ChooseSpawnPoint(Vector2 dir)
    {
        // выбираем доминирующую ось
        if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
        {
            return dir.x >= 0 ? slashSpawnPointRight : slashSpawnPointLeft;
        }
        else
        {
            return dir.y >= 0 ? slashSpawnPointUp : slashSpawnPointDown;
        }
    }

    private void ApplyOrientation(Transform slash, Vector2 dir)
    {
        // Лево/право: flip по X
        if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
        {
            // направо: scale.x положительный, налево: отрицательный
            Vector3 s = slash.localScale;
            s.x = Mathf.Abs(s.x) * (dir.x >= 0 ? 1f : -1f);
            slash.localScale = s;

            // по желанию сбрасываем вращение
            if (rotateUpDown) slash.rotation = Quaternion.identity;
        }
        else
        {
            // вверх/вниз: поворот на 90 / -90 (если твой слеш нарисован "вправо")
            if (!rotateUpDown) return;

            float angle = dir.y >= 0 ? 90f : -90f;
            slash.rotation = Quaternion.Euler(0f, 0f, angle);

            // и X оставим положительным, чтобы не было двойных флипов
            Vector3 s = slash.localScale;
            s.x = Mathf.Abs(s.x);
            slash.localScale = s;
        }
    }

    // заглушки под твои ивенты (чтобы Unity не ругался)
    public void EnableHitBox() { }
    public void DisableHitBox() { }
    public void EndAttack() { }
}
