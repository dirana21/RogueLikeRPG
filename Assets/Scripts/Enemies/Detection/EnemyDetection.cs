using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyDetection : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private LayerMask playerMask;

    private EnemyController enemy;

    void Start()
    {
        enemy = GetComponent<EnemyController>();
    }

    void Update()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, playerMask);

        if (player != null)
            enemy.SetTarget(player.transform);
        else
            enemy.SetTarget(null);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
