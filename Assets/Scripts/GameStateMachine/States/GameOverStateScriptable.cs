using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameOverState", menuName = "Runner Game/States/Game Over State")]
public class GameOverStateScriptable : GameStateScriptable
{
    public override void OnStateEnter()
    {
        Debug.Log("Entering Game Over State");
        base.OnStateEnter();

        if (stateCanvas != null)
        {
            stateCanvas.gameObject.SetActive(true);
            Debug.Log($"{stateCanvas.name} activated");
        }
        else
        {
            Debug.LogError("GameOver Canvas reference is missing!");
        }

        if (player != null)
        {
            player.enabled = false; 
        }

        Time.timeScale = 0f; 
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        Time.timeScale = 1f;

        if (player != null)
        {
            player.enabled = true; 
        }
    }
}