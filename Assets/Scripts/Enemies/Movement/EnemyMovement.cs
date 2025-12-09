using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public sealed class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float walkRadius = 1.5f;
    [SerializeField] private float idleTimeMin = 0.5f;
    [SerializeField] private float idleTimeMax = 1.5f;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.2f;     
    //[SerializeField] private float stopBuffer = 0.1f;

    [Header("Attack Logic")]
    [SerializeField] private float attackCooldown = 1.2f; // время между атаками
    private float attackTimer = 0f;
    private bool isAttacking = false;

    private Rigidbody2D rb;
    private EnemyController enemy;
    private Animator animator;

    private Vector2 currentVelocity;
    private Vector2 spawnPosition;
    private Vector2 randomTarget;
    private float idleTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemy = GetComponent<EnemyController>();
    }

    private void Start()
    {
        spawnPosition = transform.position;
        PickRandomTarget();
        idleTimer = Random.Range(idleTimeMin, idleTimeMax);
    }
    private void Update()
    {
        // 1) Обновляем анимацию ВСЕГДА
        bool isMoving = rb.velocity.sqrMagnitude > 0.01f;
        animator.SetBool("isMoving", isMoving);

        // 2) Обновляем поворот ТОЛЬКО когда движение есть
        if (isMoving)
            UpdateFacing(rb.velocity);
    }

    private void FixedUpdate()
    {
        attackTimer -= Time.fixedDeltaTime;

        if (enemy.Target != null)
        {
            Vector2 toTarget = enemy.Target.position - transform.position;
            float distance = toTarget.magnitude;

            // Если в радиусе атаки — атакуем
            if (distance <= attackRange)
            {
                currentVelocity = Vector2.zero;

                if (!isAttacking && attackTimer <= 0f)
                    StartAttack();
            }
            else if (!isAttacking)
            {
                // дальше чем attackRange — идём к игроку
                currentVelocity = MoveTowards(enemy.Target.position);
            }
        }
        else if (!isAttacking)
        {
            // Цели нет — гуляем по рандомным точкам
            currentVelocity = RandomWalk();
        }

        rb.velocity = currentVelocity;
    }

    private void StartAttack()
    {
        isAttacking = true;
        attackTimer = attackCooldown;

        animator.SetTrigger("Attack");

        // Враг не должен двигаться во время атаки
        rb.velocity = Vector2.zero;
    }
    public void EndAttack()
    {
        isAttacking = false;
    }


    // ---------------------------------------------------------
    // ------------------- RANDOM WALK LOGIC -------------------
    // ---------------------------------------------------------

    private void PickRandomTarget()
    {
        Vector2 offset = Random.insideUnitCircle * walkRadius;
        randomTarget = spawnPosition + offset;
    }

    private Vector2 RandomWalk()
    {
        // Время стоять
        if (idleTimer > 0f)
        {
            idleTimer -= Time.deltaTime;
            rb.velocity = Vector2.zero;
            return Vector2.zero;
        }

        // Движение к точке
        Vector2 vel = MoveTowards(randomTarget);

        // Достигли точки
        if (Vector2.Distance(transform.position, randomTarget) < 0.1f)
        {
            idleTimer = Random.Range(idleTimeMin, idleTimeMax);
            PickRandomTarget();
        }

        return vel;
    }

    // ---------------------------------------------------------
    // --------------------- MOVE TOWARDS -----------------------
    // ---------------------------------------------------------
    private Vector2 MoveTowards(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        return dir * moveSpeed; // БЕЗ rb.velocity !!!
    }
    private void UpdateFacing(Vector2 velocity)
    {
        float baseX = 1.3f;

        if (velocity.x > 0)
            transform.localScale = new Vector3(baseX, transform.localScale.y, 1);
        else if (velocity.x < 0)
            transform.localScale = new Vector3(-baseX, transform.localScale.y, 1);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
