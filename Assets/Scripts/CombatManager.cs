using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private PlayerCombatScript playerCombat;
    private GameStateChannel stateChannel;

    private void Awake()
    {
        if (Beacon.Instance != null)
        {
            stateChannel = Beacon.Instance.gameStateChannel;
        }
    }

    private void Start()
    {
        if (playerCombat == null)
        {
            playerCombat = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerCombatScript>();
        }
    }

    public void Update()
    {
        if (playerCombat != null)
        {
            if (playerCombat.hp < 1)  
            {
                GameStateManager.Instance.GoToGameOver();
            }
        }
    }
}