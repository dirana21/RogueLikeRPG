using UnityEngine;


public class SwordHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    private bool _active = false;

    public void EnableHitbox() => _active = true;
    public void DisableHitbox() => _active = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_active) return;

        if (other.TryGetComponent<IDamageReceiver>(out var target))
        {
            target.TakeHit(damage);
        }
    }
}