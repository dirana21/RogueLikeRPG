using UnityEngine;

public interface IMover
{
    /// <summary>Двигает персонажа в направлении (-1..1, -1..1).</summary>
    void Move(Vector2 direction, float speed);
    /// <summary>Текущая скалярная скорость для анимации (0..1+).</summary>
    float CurrentSpeed { get; }
}