using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;

    private void OnEnable() 
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        if (resumeButton != null)
            resumeButton.onClick.AddListener(HandleResume);
        if (settingsButton != null)
            settingsButton.onClick.AddListener(HandleSettings);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(HandleMainMenu);
    }

    private void HandleResume()
    {
        Debug.Log("Resume clicked");
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.GoToPlaying();
    }

    private void HandleSettings()
    {
        Debug.Log("Settings clicked!");
    }

    private void HandleMainMenu()
    {
        Debug.Log("Main Menu clicked");
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.GoToMainMenu();
    }

    private void OnDisable()
    {
        if (resumeButton != null)
            resumeButton.onClick.RemoveAllListeners();
        if (settingsButton != null)
            settingsButton.onClick.RemoveAllListeners();
        if (mainMenuButton != null)
            mainMenuButton.onClick.RemoveAllListeners();
    }
}