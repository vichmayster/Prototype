using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteState : BaseState
{
    private Canvas levelCompleteCanvas;

    public LevelCompleteState(GameStateManager manager) : base(manager) { }

    public override void Enter()
    {
        Debug.Log("Entering Level Complete State");

        levelCompleteCanvas = GameObject.FindObjectOfType<Canvas>(true);

        // Find specific canvas by name
        foreach (Canvas canvas in GameObject.FindObjectsOfType<Canvas>(true))
        {
            if (canvas.name == "LevelCompleteCanvas")
            {
                levelCompleteCanvas = canvas;
                break;
            }
        }

        if (levelCompleteCanvas != null)
        {
            Debug.Log($"Found canvas: {levelCompleteCanvas.name}, currently active: {levelCompleteCanvas.gameObject.activeInHierarchy}");

            // Enable the canvas and all its components
            levelCompleteCanvas.gameObject.SetActive(true);
            levelCompleteCanvas.enabled = true;

            var canvasGroup = levelCompleteCanvas.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }
        else
        {
            Debug.LogError("Could not find LevelCompleteCanvas!");
        }

        // Pause the game
        Time.timeScale = 0f;

        // Disable player input
        if (player != null)
        {
            player.enabled = false;
        }
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) // Next level
        {
            Time.timeScale = 1f;
            gameStateManager.GoToPlaying();
        }
        else if (Input.GetKeyDown(KeyCode.M)) // Main menu
        {
            Time.timeScale = 1f;
            gameStateManager.GoToMainMenu();
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Level Complete State");

        if (levelCompleteCanvas != null)
        {
            levelCompleteCanvas.gameObject.SetActive(false);
        }

        Time.timeScale = 1f;
    }
}