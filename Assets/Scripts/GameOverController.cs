using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button returnToMenuButton;

    private void OnEnable()
    {
        SetupButtons();
    }
    private void SetupButtons()
    {
        if (retryButton != null)
        {
            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(HandleRetry);
        }

        if (returnToMenuButton != null)
        {
            returnToMenuButton.onClick.RemoveAllListeners();
            returnToMenuButton.onClick.AddListener(HandleReturnToMenu);
        }
    }

    private void HandleRetry()
    {
        Debug.Log("Retry clicked");
        Time.timeScale = 1f;

        if (GameStateManager.Instance?.PlayerReference != null)
        {
            GameStateManager.Instance.PlayerReference.ResetPlayer();
        }

        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.GoToPlaying();
        }
    }

    private void HandleReturnToMenu()
    {
        Debug.Log("Return to Menu clicked");
        Time.timeScale = 1f;

        if (GameStateManager.Instance != null)
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.ResetToFirstLevel();
            }
            GameStateManager.Instance.GoToMainMenu();
        }
    }
}