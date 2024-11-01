using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawner_1 : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject prefab2;
    [SerializeField] GameObject EnemySpawner;
    [SerializeField] GameObject EnemySpawner2;
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

        spawnLocation = EnemySpawner2.transform.position;
        SkeletonEnemy = Instantiate(prefab2, spawnLocation, Quaternion.identity);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
