using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationDriver : MonoBehaviour
{
    private Animator _anim;
    void Awake() => _anim = GetComponent<Animator>();
    public void SetMoveSpeed(float speed) => _anim.SetFloat("Speed", speed);
    public void PlayAttack() => _anim.SetTrigger("Attack");
    public void PlayTakeDamage() => _anim.SetTrigger("TakeDamage");
    public void PlayDeath() => _anim.SetBool("IsDead", true);
}