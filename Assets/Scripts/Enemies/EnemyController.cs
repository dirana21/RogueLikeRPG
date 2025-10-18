using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // === НАСТРОЙКИ ДВИЖЕНИЯ И ПАТРУЛЯ ===
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public Transform[] waypoints;     // Точки маршрута
    private int currentWaypointIndex = 0;

    [Header("Waypoint Pause")]
    public float waitTime = 1.5f;     // Сколько секунд стоять в точке
    private float waitTimer = 0f;     // Текущий таймер
    private bool isWaiting = false;   // Флаг, находимся ли мы в режиме паузы

    // === ТЕНЬ (НОВЫЙ БЛОК) ===
    [Header("Shadow Correction")]
    public Transform shadowTransform; // Ссылка на Transform дочернего объекта Shadow
    public float shadowOffsetX = 0.1f; // Смещение тени по X (положительное, например, 0.1)
    // ===========================

    // === ЗДОРОВЬЕ ===
    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    // --- Добавленные компоненты для анимации и визуала ---
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    // ---------------------------------------------------

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Получаем компоненты, которые нам нужны для управления
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            // Если точек нет, останавливаем анимацию ходьбы
            if (animator != null)
                animator.SetBool("IsWalking", false);
            return;
        }

        // --- ЛОГИКА ПАУЗЫ (ОСМАТРИВАНИЯ) ---
        if (isWaiting)
        {
            waitTimer -= Time.fixedDeltaTime;

            if (waitTimer <= 0f)
            {
                isWaiting = false;

                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                    currentWaypointIndex = 0;
            }

            return;
        }
        // --- КОНЕЦ ЛОГИКИ ПАУЗЫ ---


        Transform targetPoint = waypoints[currentWaypointIndex];

        // Двигаемся к текущей точке
        Vector2 direction = (targetPoint.position - transform.position).normalized;

        // 1. УПРАВЛЕНИЕ ФЛИППИНГОМ (Поворотом спрайта)
        float currentShadowOffsetX = shadowOffsetX;

        if (direction.x > 0.01f) // Идем вправо
        {
            spriteRenderer.flipX = false;
            currentShadowOffsetX = shadowOffsetX; // Тень смещена вправо
        }
        else if (direction.x < -0.01f) // Идем влево
        {
            spriteRenderer.flipX = true;
            currentShadowOffsetX = -shadowOffsetX; // Тень смещена влево (симметрично)
        }

        // КОРРЕКЦИЯ ПОЗИЦИИ ТЕНИ
        if (shadowTransform != null)
        {
            // Здесь мы устанавливаем локальную позицию X тени, сохраняя Y и Z
            shadowTransform.localPosition = new Vector3(
                currentShadowOffsetX,
                shadowTransform.localPosition.y,
                shadowTransform.localPosition.z
            );
        }
        // КОНЕЦ КОРРЕКЦИИ ТЕНИ


        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        // 2. УПРАВЛЕНИЕ АНИМАЦИЕЙ
        if (animator != null)
            animator.SetBool("IsWalking", direction.sqrMagnitude > 0.01f);


        // Проверяем, дошёл ли враг до точки
        float distance = Vector2.Distance(transform.position, targetPoint.position);
        if (distance < 0.1f)
        {
            isWaiting = true;
            waitTimer = waitTime;

            if (animator != null)
                animator.SetBool("IsWalking", false);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Визуализация маршрута (в редакторе)
        Gizmos.color = Color.red;
        if (waypoints != null)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] != null)
                    Gizmos.DrawSphere(waypoints[i].position, 0.1f);

                if (i + 1 < waypoints.Length && waypoints[i] != null && waypoints[i + 1] != null)
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }
}