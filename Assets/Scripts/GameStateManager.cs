using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [Header("State Tracking")]
    [SerializeField] private GameStateScriptable currentState;
    [SerializeField] private string currentStateName;

    [Header("State Configuration")]
    [SerializeField] private GameStateChannel stateChannel;
    [SerializeField] private MainMenuStateScriptable menuStateScriptable;
    [SerializeField] private PausedStateScriptable pausedStateScriptable;
    [SerializeField] private PlayingStateScriptable playingStateScriptable;
    [SerializeField] private LevelCompleteStateScriptable levelCompleteStateScriptable;
    [SerializeField] private GameOverStateScriptable gameOverStateScriptable;
    [SerializeField] private CombatStateScriptable combatStateScriptable;


    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement; 
    [SerializeField] private CameraScript cameraScript;

    public PlayerMovement PlayerReference => playerMovement;
    public CameraScript CameraReference => cameraScript;
    public GameStateScriptable CurrentState => currentState; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeScriptableStates();

            if (stateChannel != null)
            {
                stateChannel.StateEnter += HandleStateEnter;
                stateChannel.GetCurrentState += GetCurrentState;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("GameStateManager Start");
        DisableAllCanvases();
        if (menuStateScriptable != null)
        {
            ChangeState(menuStateScriptable);
        }
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnStateUpdate();
        }
    }

    private void InitializeScriptableStates()
    {
        if (menuStateScriptable != null) menuStateScriptable.Initialize(playerMovement, cameraScript);
        if (playingStateScriptable != null) playingStateScriptable.Initialize(playerMovement, cameraScript);
        if (pausedStateScriptable != null) pausedStateScriptable.Initialize(playerMovement, cameraScript);
        if (levelCompleteStateScriptable != null) levelCompleteStateScriptable.Initialize(playerMovement, cameraScript);
        if (gameOverStateScriptable != null) gameOverStateScriptable.Initialize(playerMovement, cameraScript);
        if (combatStateScriptable != null) combatStateScriptable.Initialize(playerMovement, cameraScript);

    }

    private void ChangeState(GameStateScriptable newState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
            stateChannel?.StateExited(currentState);
        }

        currentState = newState;
        currentStateName = newState.stateName; 
        currentState.OnStateEnter();
        stateChannel?.StateEntered(currentState);
    }

    private void HandleStateEnter(GameStateScriptable newState) 
    {
        if (newState != currentState)
        {
            ChangeState(newState);
        }
    }

   
    private GameStateScriptable GetCurrentState()
    {
        return currentState;
    }
    public void GoToMainMenu()
    {
        Debug.Log("GoToMainMenu called");
        if (menuStateScriptable != null)
        {
            ChangeState(menuStateScriptable);
        }
    }

    public void GoToPlaying()
    {
        Debug.Log("GoToPlaying called");
        if (playingStateScriptable != null)
        {
            ChangeState(playingStateScriptable);
        }
    }

    public void GoToPaused()
    {
        Debug.Log("GoToPaused called");
        if (pausedStateScriptable != null)
        {
            ChangeState(pausedStateScriptable);
        }
    }

    public void GoToLevelComplete()
    {
        Debug.Log("GoToLevelComplete called");
        if (levelCompleteStateScriptable != null)
        {
            ChangeState(levelCompleteStateScriptable);
        }
    }


    public void GoToGameOver()
    {
        Debug.Log("GoToGameOver called");
        if (gameOverStateScriptable != null)
        {
            ChangeState(gameOverStateScriptable);
        }
    }

    public void GoToCombat()
    {
        Debug.Log("GoToCombat called");
        if (combatStateScriptable != null)
        {
            ChangeState(combatStateScriptable);
        }
    }


    private void DisableAllCanvases()
    {
        Debug.Log("Disabling all canvases");
        var pauseCanvas = GameObject.Find("PauseMenuCanvas")?.GetComponent<Canvas>();
        var levelCompleteCanvas = GameObject.Find("LevelCompleteCanvas")?.GetComponent<Canvas>();
        var mainMenuCanvas = GameObject.Find("MainMenuCanvas")?.GetComponent<Canvas>();
        var gameOverCanvas = GameObject.Find("GameOverCanvas")?.GetComponent<Canvas>();

        // First disable all canvases
        if (pauseCanvas != null)
        {
            pauseCanvas.gameObject.SetActive(false);
            Debug.Log("PauseMenuCanvas disabled");
        }
        if (levelCompleteCanvas != null)
        {
            levelCompleteCanvas.gameObject.SetActive(false);
            Debug.Log("LevelCompleteCanvas disabled");
        }
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(false);
            Debug.Log("GameOverCanvas disabled");
        }
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.gameObject.SetActive(true);  // This one starts enabled
            Debug.Log("MainMenuCanvas enabled");
        }
    }

    public PlayerMovement GetPlayer() => playerMovement;
    public CameraScript GetCamera() => cameraScript;

    private void OnDestroy()
    {
        if (stateChannel != null)
        {
            stateChannel.StateEnter -= HandleStateEnter;
            stateChannel.GetCurrentState -= GetCurrentState;
        }
    }
}