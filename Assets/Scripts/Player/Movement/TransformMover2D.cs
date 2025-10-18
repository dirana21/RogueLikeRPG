using UnityEngine;

public class TransformMover2D : MonoBehaviour, IMover
{
    public float CurrentSpeed { get; private set; }

    public void Move(Vector2 direction, float speed)
    {
        Vector3 dir = direction.normalized;
        transform.position += (Vector3)dir * speed * Time.deltaTime;
        CurrentSpeed = dir.magnitude; // 0..1
    }
}