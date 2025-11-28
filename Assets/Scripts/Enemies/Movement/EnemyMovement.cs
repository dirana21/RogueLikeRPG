using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public sealed class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float walkRadius = 1.5f;
    [SerializeField] private float idleTimeMin = 0.5f;
    [SerializeField] private float idleTimeMax = 1.5f;

    private Rigidbody2D rb;
    private EnemyController enemy;
    private Animator animator;

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

    private void FixedUpdate()
    {
        Vector2 velocity;

        // Преследуем игрока
        if (enemy.Target != null)
        {
            velocity = MoveTowards(enemy.Target.position);
        }
        else
        {
            velocity = RandomWalk();
        }
        // Передаём анимацию
        animator.SetBool("isMoving", velocity.sqrMagnitude > 0.01f);
        UpdateFacing(velocity);
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
        Vector2 velocity = dir * moveSpeed;

        rb.velocity = velocity;
        return velocity;
    }
    private void UpdateFacing(Vector2 velocity)
    {
        if (velocity.sqrMagnitude < 0.01f)
            return;

        float baseX = 1.3f; // или Mathf.Abs(transform.localScale.x);

        if (velocity.x > 0)
            transform.localScale = new Vector3(baseX, transform.localScale.y, 1);
        else if (velocity.x < 0)
            transform.localScale = new Vector3(-baseX, transform.localScale.y, 1);
    }
}
