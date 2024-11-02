using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyTrigger : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Trophy initialized");
        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(1f, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Trophy triggered by: {other.gameObject.name} with tag: {other.tag}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player tag detected, attempting to trigger level complete");
            if (GameStateManager.Instance != null)
            {
                Debug.Log("GameStateManager found, calling GoToLevelComplete");
                GameStateManager.Instance.GoToLevelComplete();
            }
            else
            {
                Debug.LogError("GameStateManager.Instance is null!");
            }
        }
    }
}