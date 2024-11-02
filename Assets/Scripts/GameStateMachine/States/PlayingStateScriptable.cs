using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayingState", menuName = "Runner Game/States/Playing State")]
public class PlayingStateScriptable : GameStateScriptable
{
    private Action playerDeathHandler;

    public override void OnStateEnter()
    {
        Debug.Log("Entering Playing State");
        base.OnStateEnter();

        Time.timeScale = 1f;

        if (player != null)
        {
            player.gameObject.SetActive(true);
            player.enabled = true;
            player.ResetPlayer();

            playerDeathHandler = () => GameStateManager.Instance.GoToGameOver();
            player.OnPlayerDeath += playerDeathHandler;
        }

        // Setup camera
        if (camera != null)
        {
            camera.gameObject.SetActive(true);
        }

        DisableUICanvases();
    }

    public override void OnStateUpdate()
    {
        // Check for pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed in Playing State");
            GameStateManager.Instance.GoToPaused();
            return;
        }

        // Check for game over
        if (player != null && player.hp < 1)
        {
            GameStateManager.Instance.GoToGameOver();
            return;
        }
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Playing State");
        base.OnStateExit();

        if (player != null && playerDeathHandler != null)
        {
            player.OnPlayerDeath -= playerDeathHandler;
        }
    }

    private void DisableUICanvases()
    {
        var mainMenuCanvas = GameObject.Find("MainMenuCanvas")?.GetComponent<Canvas>();
        var pauseMenuCanvas = GameObject.Find("PauseMenuCanvas")?.GetComponent<Canvas>();
        var gameOverCanvas = GameObject.Find("GameOverCanvas")?.GetComponent<Canvas>();
        var levelCompleteCanvas = GameObject.Find("LevelCompleteCanvas")?.GetComponent<Canvas>();

        if (mainMenuCanvas != null) mainMenuCanvas.gameObject.SetActive(false);
        if (pauseMenuCanvas != null) pauseMenuCanvas.gameObject.SetActive(false);
        if (gameOverCanvas != null) gameOverCanvas.gameObject.SetActive(false);
        if (levelCompleteCanvas != null) levelCompleteCanvas.gameObject.SetActive(false);
    }
}
