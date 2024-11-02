using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        // Find buttons if not assigned
        if (playButton == null)
            playButton = transform.Find("MenuContainer/PlayButton")?.GetComponent<Button>();
        if (settingsButton == null)
            settingsButton = transform.Find("MenuContainer/SettingsButton")?.GetComponent<Button>();
        if (quitButton == null)
            quitButton = transform.Find("MenuContainer/QuitButton")?.GetComponent<Button>();

        // Add listeners
        if (playButton != null)
            playButton.onClick.AddListener(HandlePlayClick);
        if (settingsButton != null)
            settingsButton.onClick.AddListener(HandleSettingsClick);
        if (quitButton != null)
            quitButton.onClick.AddListener(HandleQuitClick);
    }

    private void HandlePlayClick()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.GoToPlaying();
    }

    private void HandleSettingsClick()
    {
        Debug.Log("Settings clicked!");
    }

    private void HandleQuitClick()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}