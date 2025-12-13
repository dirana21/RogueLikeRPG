using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AutoDestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.35f;
    private float t;

    private void OnEnable() => t = 0f;

    private void Update()
    {
        t += Time.deltaTime;
        if (t >= lifeTime) Destroy(gameObject);
    }
}
