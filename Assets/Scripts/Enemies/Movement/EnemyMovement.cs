using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float walkRadius = 1.5f;  // насколько далеко он уходит от своей точки
    [SerializeField] private float idleTimeMin = 0.5f;
    [SerializeField] private float idleTimeMax = 1.5f;

    private Vector2 spawnPosition;
    private Vector2 randomTarget;
    private float idleTimer = 0f;

    private Rigidbody2D rb;
    private EnemyController enemy;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<EnemyController>();

        spawnPosition = transform.position;
        PickRandomTarget();
        idleTimer = Random.Range(idleTimeMin, idleTimeMax);
    }

    void FixedUpdate()
    {
        // Если игрок найден — преследуем
        if (enemy.Target != null)
        {
            MoveTowards(enemy.Target.position);
            return;
        }

        // Иначе — случайное блуждание
        RandomWalk();
    }

    private void PickRandomTarget()
    {
        Vector2 randomOffset = Random.insideUnitCircle * walkRadius;
        randomTarget = spawnPosition + randomOffset;
    }

    private void RandomWalk()
    {
        // Если стоим на месте (idle)
        if (idleTimer > 0f)
        {
            idleTimer -= Time.deltaTime;
            return;
        }

        // Движемся к случайной точке
        MoveTowards(randomTarget);

        // Когда достигли точки — останавливаемся и выбираем другую
        if (Vector2.Distance(transform.position, randomTarget) < 0.1f)
        {
            idleTimer = Random.Range(idleTimeMin, idleTimeMax);
            PickRandomTarget();
        }
    }

    private void MoveTowards(Vector2 targetPos)
    {
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }
}
