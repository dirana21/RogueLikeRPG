using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [Header("Attack settings")]
    [SerializeField] private float attackRadius = 0.8f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attackPoint;

    public void Hit()
    {
        if (!attackPoint)
        {
            Debug.LogWarning("AttackPoint not assigned!");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            enemyLayer
        );

        foreach (var hit in hits)
        {
            var damageReceiver = hit.GetComponent<IDamageReceiver>();
            if (damageReceiver != null)
            {
                damageReceiver.TakeHit(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!attackPoint) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}