using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;

    private void OnEnable()
    {
        if (retryButton != null)
            retryButton.onClick.AddListener(HandleRetry);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(HandleMainMenu);
    }

    private void HandleRetry()
    {
        GameStateManager.Instance?.GoToPlaying();
    }

    private void HandleMainMenu()
    {
        GameStateManager.Instance?.GoToMainMenu();
    }

    private void OnDisable()
    {
        if (retryButton != null)
            retryButton.onClick.RemoveListener(HandleRetry);
        if (mainMenuButton != null)
            mainMenuButton.onClick.RemoveListener(HandleMainMenu);
    }
}