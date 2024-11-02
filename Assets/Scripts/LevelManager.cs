using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public int CurrentLevel { get; private set; } = 1;

    [SerializeField] private string[] levelScenes = new string[] { "Level1", "Level2", "Level3" };
    private readonly int maxLevels = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadNextLevel()
    {
        CurrentLevel++;
        Debug.Log($"Loading level {CurrentLevel}");

        if (CurrentLevel <= maxLevels)
        {
            // Load the next scene
            string nextScene = levelScenes[CurrentLevel - 1];
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            // Game completed, return to main menu
            Debug.Log("You've finished the game!");
            CurrentLevel = 1;
            GameStateManager.Instance.GoToMainMenu();
        }
    }

    public void ResetToFirstLevel()
    {
        CurrentLevel = 1;
        SceneManager.LoadScene(levelScenes[0]);
    }

    private void ResetCurrentLevel()
    {
        // Reset player position
        var player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.ResetPlayer();
            player.transform.position = GetLevelStartPosition();
        }
    }

    private Vector3 GetLevelStartPosition()
    {
        switch (CurrentLevel)
        {
            case 1:
                return new Vector3(0, 0, 0);
            case 2:
                return new Vector3(0, 0, 0);
            case 3:
                return new Vector3(0, 0, 0);
            default:
                return new Vector3(0, 0, 0);
        }
    }

    public string GetCurrentLevelName()
    {
        return $"Level {CurrentLevel}";
    }
}