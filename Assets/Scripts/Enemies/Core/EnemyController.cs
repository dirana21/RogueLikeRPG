using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStatsMono))]
public sealed class EnemyController : MonoBehaviour
{
    public Transform Target { get; private set; }
    public CharacterStatsMono Stats { get; private set; }

    private void Awake()
    {
        Stats = GetComponent<CharacterStatsMono>();
    }

    public void SetTarget(Transform t)
    {
        Target = t;
    }
}
