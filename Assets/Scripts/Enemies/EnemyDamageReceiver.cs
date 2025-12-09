using UnityEngine;
  

[RequireComponent(typeof(CharacterStatsMono))]
public class EnemyDamageReceiver : MonoBehaviour, IDamageReceiver
{
    private CharacterStatsMono statsMono;
    private Animator anim;

    private void Awake()
    {
        statsMono = GetComponent<CharacterStatsMono>();
        anim      = GetComponent<Animator>();

        if (statsMono != null)
            statsMono.Model.Health.OnDepleted += OnDeath;
    }

    public void TakeHit(float damage)
    {
        if (statsMono == null) return;

        statsMono.Model.Health.Add(-damage);
        Debug.Log($"{name} получил {damage} урона. HP = {statsMono.Model.Health.Current}");
    }

    private void OnDeath()
    {
        Debug.Log($"{name} умер");

        if (anim != null)
            anim.SetBool("isDead", true); // или SetTrigger("Death") — как у вас принято

        Destroy(gameObject, 1.5f);
    }
}