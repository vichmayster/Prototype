using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawner_1 : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject EnemySpawner;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovment.enemySpawn += HandleEnemySpawn;
    }

    public void HandleEnemySpawn()
    {
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        Vector3 spawnLocation = EnemySpawner.transform.position;
        GameObject SkeletonEnemy = Instantiate(prefab,spawnLocation, Quaternion.identity);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
