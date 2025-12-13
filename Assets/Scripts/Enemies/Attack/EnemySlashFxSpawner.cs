using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemySlashFxSpawner : MonoBehaviour
{
    [SerializeField] private GameObject slashFxPrefab;
    [SerializeField] private Transform spawnPoint;

    public void SpawnSlashFx()
    {
        if (slashFxPrefab == null || spawnPoint == null) return;
        Instantiate(slashFxPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
