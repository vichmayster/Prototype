using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState : BaseState
{
    private new PlayerMovement player => gameStateManager.GetPlayer();
    private new CameraScript camera => gameStateManager.GetCamera();

    public PlayingState(GameStateManager manager) : base(manager) { }

    public override void Enter()
    {
        Debug.Log("Entering Playing State");

        // Setup player
        if (player != null)
        {
            player.gameObject.SetActive(true);
            player.enabled = true;
            player.ResetPlayer();
            player.OnPlayerDeath += HandlePlayerDeath;
        }

        // Setup camera
        if (camera != null)
            camera.gameObject.SetActive(true);

        // Ensure game is running
        Time.timeScale = 1f;

        DisableUICanvases();
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

    public override void Update()
    {
        // Check for pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed in Playing State");
            gameStateManager.GoToPaused();
            return;
        }

        // Check for game over
        if (player != null && player.hp < 1)
        {
            gameStateManager.GoToGameOver();
            return;
        }
    }

    private void HandlePlayerDeath()
    {
        gameStateManager.GoToGameOver();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Playing State");

        if (player != null)
        {
            player.OnPlayerDeath -= HandlePlayerDeath;
        }
    }
}