using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        if (playButton == null)
            playButton = transform.Find("MenuContainer/PlayButton")?.GetComponent<Button>();
        if (quitButton == null)
            quitButton = transform.Find("MenuContainer/QuitButton")?.GetComponent<Button>();

        if (playButton != null)
            playButton.onClick.AddListener(HandlePlayClick);
        if (quitButton != null)
            quitButton.onClick.AddListener(HandleQuitClick);
    }

    private void HandlePlayClick()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.GoToPlaying();
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