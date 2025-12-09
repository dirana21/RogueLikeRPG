using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [Header("Attack settings")]
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float attackDistance = 0.8f;   // насколько далеко от игрока
    [SerializeField] private float attackRadius   = 0.6f;   // радиус удара
    [SerializeField] private LayerMask enemyMask;           // сюда поставим слой Enemy

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            if (anim != null)
                anim.SetTrigger("Attack");   

            DoHit();
        }
    }

    private void DoHit()
    {
        
        Vector2 center = transform.position;
        center += (Vector2)transform.right * attackDistance;  // направо от игрока

        // ищем все коллайдеры врагов в зоне
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, attackRadius, enemyMask);

        foreach (var hit in hits)
        {
            var receiver = hit.GetComponent<IDamageReceiver>();
            if (receiver != null)
            {
                receiver.TakeHit(attackDamage);
            }
        }
    }

    // чтобы видеть зону удара в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector2 center = transform.position;
        center += (Vector2)transform.right * attackDistance;

        Gizmos.DrawWireSphere(center, attackRadius);
    }
}