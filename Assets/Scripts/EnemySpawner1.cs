using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner1 : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject prefab2;
    [SerializeField] GameObject EnemySpawner;
    [SerializeField] GameObject EnemySpawner2;
    bool isSpawned = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && !isSpawned)
        {
            isSpawned = true;
            if (prefab != null && EnemySpawner != null)
            {
                Vector3 spawnLocation = EnemySpawner.transform.position;
                GameObject SkeletonEnemy = Instantiate(prefab, spawnLocation, Quaternion.identity);
            }

            if (prefab2 != null && EnemySpawner2 != null)
            {
                Vector3 spawnLocation = EnemySpawner2.transform.position;
                GameObject SkeletonEnemy = Instantiate(prefab2, spawnLocation, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }


}