using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GameStateScriptable : ScriptableObject
{
    [Header("State Settings")]
    public string stateName;
    public bool isCurrentState;
    public bool canMenu = true;

    [Header("References")]
    [SerializeField] protected Canvas stateCanvas;
    protected PlayerMovement player;
    protected CameraScript camera;
    protected GameStateManager gameStateManager;

    public virtual void Initialize(PlayerMovement playerRef, CameraScript cameraRef)
    {
        player = playerRef;
        camera = cameraRef;
        gameStateManager = GameStateManager.Instance;
    }

    public virtual void OnStateEnter()
    {
        isCurrentState = true;
        if (stateCanvas != null)
        {
            stateCanvas.gameObject.SetActive(true);
        }
    }

    public virtual void OnStateUpdate() { }

    public virtual void OnStateExit()
    {
        isCurrentState = false;
        if (stateCanvas != null)
        {
            stateCanvas.gameObject.SetActive(false);
        }
    }
}