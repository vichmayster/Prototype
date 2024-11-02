using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedState : BaseState
{
    private Canvas pauseMenuCanvas;

    public PausedState(GameStateManager manager) : base(manager) { }

    public override void Enter()
    {
        Debug.Log("Entering Paused State");

        foreach (Canvas canvas in GameObject.FindObjectsOfType<Canvas>(true))
        {
            if (canvas.name == "PauseMenuCanvas")
            {
                pauseMenuCanvas = canvas;
                break;
            }
        }

        if (pauseMenuCanvas != null)
        {
            Debug.Log($"Found pause canvas: {pauseMenuCanvas.name}, currently active: {pauseMenuCanvas.gameObject.activeInHierarchy}");

            // Enable the canvas and all its components
            pauseMenuCanvas.gameObject.SetActive(true);
            pauseMenuCanvas.enabled = true;
        }
        else
        {
            Debug.LogError("PauseMenuCanvas not found!");
        }

        // Freeze the game
        Time.timeScale = 0f;

        if (player != null)
        {
            player.enabled = false;
        }
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed in pause state");
            gameStateManager.GoToPlaying();
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Paused State");

        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.gameObject.SetActive(false);
        }

        Time.timeScale = 1f;

        // Re-enable player
        if (player != null)
        {
            player.enabled = true;
        }
    }
}