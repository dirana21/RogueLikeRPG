using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public sealed class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float walkRadius = 1.5f;
    [SerializeField] private float idleTimeMin = 0.5f;
    [SerializeField] private float idleTimeMax = 1.5f;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.2f;

    [Header("Attack Logic")]
    [SerializeField] private float attackCooldown = 1.2f;

    [Header("Attack Origin")]
    [SerializeField] private Transform attackOrigin;

    private float attackTimer = 0f;
    private bool isAttacking = false;

    // направление на цель для слеша (Right/Left/Up/Down)
    private Vector2 lastAimDir = Vector2.right;
    public Vector2 LastAimDir => lastAimDir;

    private Rigidbody2D rb;
    private EnemyController enemy;
    private Animator animator;
    private SpriteRenderer sr;

    private Vector2 currentVelocity;
    private Vector2 spawnPosition;
    private Vector2 randomTarget;
    private float idleTimer = 0f;

    private float baseScaleXAbs; // запоминаем твой масштаб (1.3), но только положительный

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        enemy = GetComponent<EnemyController>();

        if (attackOrigin == null)
            attackOrigin = transform;

        baseScaleXAbs = Mathf.Abs(transform.localScale.x);
    }

    private void Start()
    {
        spawnPosition = rb.position;
        PickRandomTarget();
        idleTimer = Random.Range(idleTimeMin, idleTimeMax);
    }

    private void Update()
    {
        bool isMoving = rb.velocity.sqrMagnitude > 0.01f;
        animator.SetBool("isMoving", isMoving);

        // Фейсинг обновляем:
        // - при движении по velocity
        // - если стоим и есть цель, то по lastAimDir (чтобы слеш не появлялся "не там")
        if (isMoving)
            UpdateFacing(rb.velocity);
        else if (enemy != null && enemy.Target != null)
            UpdateFacing(lastAimDir);
    }

    private void FixedUpdate()
    {
        attackTimer -= Time.fixedDeltaTime;

        // позиция, откуда меряем радиус атаки (исправляет "сверху не достает")
        Vector2 originPos = attackOrigin ? (Vector2)attackOrigin.position : rb.position;

        if (enemy != null && enemy.Target != null)
        {
            Vector2 toTarget = (Vector2)enemy.Target.position - originPos;

            // сохраняем направление атаки (для SpawnSlashFX)
            if (toTarget.sqrMagnitude > 0.0001f)
                lastAimDir = toTarget.normalized;

            float distance = toTarget.magnitude;

            if (distance <= attackRange)
            {
                currentVelocity = Vector2.zero;

                if (!isAttacking && attackTimer <= 0f)
                    StartAttack();
            }
            else if (!isAttacking)
            {
                currentVelocity = MoveTowards(enemy.Target.position);
            }
            else
            {
                currentVelocity = Vector2.zero;
            }
        }
        else if (!isAttacking)
        {
            currentVelocity = RandomWalk();
        }
        else
        {
            currentVelocity = Vector2.zero;
        }

        rb.velocity = currentVelocity;
    }

    private void StartAttack()
    {
        isAttacking = true;
        attackTimer = attackCooldown;

        animator.SetTrigger("Attack");
        rb.velocity = Vector2.zero;
    }

    // Animation Event в конце атаки
    public void EndAttack()
    {
        isAttacking = false;
    }

    // ------------------- RANDOM WALK LOGIC -------------------

    private void PickRandomTarget()
    {
        Vector2 offset = Random.insideUnitCircle * walkRadius;
        randomTarget = spawnPosition + offset;
    }

    private Vector2 RandomWalk()
    {
        if (idleTimer > 0f)
        {
            idleTimer -= Time.deltaTime;
            return Vector2.zero;
        }

        Vector2 vel = MoveTowards(randomTarget);

        if (Vector2.Distance(rb.position, randomTarget) < 0.1f)
        {
            idleTimer = Random.Range(idleTimeMin, idleTimeMax);
            PickRandomTarget();
        }

        return vel;
    }

    private Vector2 MoveTowards(Vector2 target)
    {
        Vector2 dir = (target - rb.position).normalized;
        return dir * moveSpeed;
    }

    /// <summary>
    /// Корень держим с положительным scaleX, а разворот делаем через SpriteRenderer.flipX.
    /// Это не ломает дочерние SpawnPoint'ы и FX.
    /// </summary>
    private void UpdateFacing(Vector2 dirOrVelocity)
    {
        if (dirOrVelocity.sqrMagnitude < 0.0001f)
            return;

        // scale всегда положительный (чтобы дети не зеркалились)
        var s = transform.localScale;
        s.x = baseScaleXAbs;
        transform.localScale = s;

        // flip только визуальный
        if (sr != null)
            sr.flipX = dirOrVelocity.x < -0.01f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 p = attackOrigin ? attackOrigin.position : transform.position;
        Gizmos.DrawWireSphere(p, attackRange);
    }
}
