using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PausedState", menuName = "Runner Game/States/Paused State")]
public class PausedStateScriptable : GameStateScriptable
{
    public override void OnStateEnter()
    {
        Debug.Log("Entering Paused State");
        base.OnStateEnter();

        if (stateCanvas == null)
        {
            stateCanvas = GameObject.Find("PauseMenuCanvas")?.GetComponent<Canvas>();
        }

        if (stateCanvas != null)
        {
            stateCanvas.gameObject.SetActive(true);
            Debug.Log("PauseMenuCanvas activated");
        }

        Time.timeScale = 0f;
        if (player != null)
        {
            player.enabled = false;
        }
    }

    public override void OnStateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed in pause state");
            Time.timeScale = 1f;
            if (player != null)
            {
                player.enabled = true;
            }
            GameStateManager.Instance.GoToPlaying();
        }
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Paused State");
        base.OnStateExit();

       
        Time.timeScale = 1f;

        if (player != null)
        {
            player.enabled = true;
        }

        if (stateCanvas != null)
        {
            stateCanvas.gameObject.SetActive(false);
        }
    }
}