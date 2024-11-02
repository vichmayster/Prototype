using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    [SerializeField] private LevelChannel levelChannel;
    public int CurrentLevel { get; private set; } = 1;

    [SerializeField] private string[] levelScenes = new string[] { "Level1", "Level2", "Level3" };
    private readonly int maxLevels = 3;

    private void OnEnable()
    {
        if (levelChannel != null)
        {
            Debug.Log("Setting up level channel listener");
            levelChannel.OnLoadNextLevel.AddListener(LoadNextLevel);
        }
        else
        {
            Debug.LogError("Level Channel is null in LevelManager!");
        }
    }

    private void OnDisable()
    {
        levelChannel.OnLoadNextLevel.RemoveListener(LoadNextLevel);
    }

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
        Debug.Log($"LoadNextLevel called. Current level: {CurrentLevel}");
        CurrentLevel++;
        Debug.Log($"Loading level {CurrentLevel}");

        if (CurrentLevel <= maxLevels)
        {
            string nextScene = levelScenes[CurrentLevel - 1];
            Debug.Log($"Loading scene: {nextScene}");
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.Log("Game completed!");
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