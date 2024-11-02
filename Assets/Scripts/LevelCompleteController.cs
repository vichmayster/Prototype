using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCompleteController : MonoBehaviour
{
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI levelCompleteText;
    [SerializeField] private LevelChannel levelChannel;

    private void OnEnable()
    {
        SetupButtons();
        UpdateLevelText();

        if (LevelManager.Instance != null && nextLevelButton != null)
        {
            nextLevelButton.gameObject.SetActive(LevelManager.Instance.CurrentLevel < 3);
        }
    }

    private void SetupButtons()
    {
        Debug.Log("Setting up level complete buttons");

        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(HandleNextLevel);
            Debug.Log("Next Level button listener added");
        }
        else
        {
            Debug.LogError("Next Level Button reference missing!");
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(HandleMainMenu);
        }
    }

    private void HandleNextLevel()
    {
        Debug.Log("Next Level Button Clicked");
        if (levelChannel == null)
        {
            Debug.LogError("Level Channel is null!");
            return;
        }

        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }

        Time.timeScale = 1f;
        levelChannel.OnLoadNextLevel.Invoke();
        Debug.Log("OnLoadNextLevel Invoked");
    }

    private void UpdateLevelText()
    {
        if (levelCompleteText != null && LevelManager.Instance != null)
        {
            if (LevelManager.Instance.CurrentLevel >= 3)
            {
                levelCompleteText.text = "Game Completed!";
            }
            else
            {
                levelCompleteText.text = $"Level {LevelManager.Instance.CurrentLevel} Complete!";
            }
        }
    }

    private void HandleMainMenu()
    {
        Time.timeScale = 1f;
        GameStateManager.Instance.GoToMainMenu();
    }

    private void OnDisable()
    {
        if (nextLevelButton != null)
            nextLevelButton.onClick.RemoveAllListeners();
        if (mainMenuButton != null)
            mainMenuButton.onClick.RemoveAllListeners();
    }
}
