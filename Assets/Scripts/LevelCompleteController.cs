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

    private void OnEnable()
    {
        SetupButtons();
        UpdateLevelText();

        if (LevelManager.Instance != null && nextLevelButton != null && LevelManager.Instance.CurrentLevel >= 3)
        {
            nextLevelButton.gameObject.SetActive(false);
        }
    }

    private void SetupButtons()
    {
        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(HandleNextLevel);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(HandleMainMenu);
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

        private void HandleNextLevel()
    {
        Time.timeScale = 1f;
        LevelManager.Instance.LoadNextLevel();
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