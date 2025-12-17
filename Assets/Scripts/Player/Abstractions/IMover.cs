using UnityEngine;

public interface IMover
{
    void Move(Vector2 direction, float speed);
    float CurrentSpeed { get; }
}