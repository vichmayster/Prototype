using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI; 

public abstract class BaseState
{
    protected GameStateManager gameStateManager;
    protected PlayerMovement player => gameStateManager.GetPlayer();
    protected CameraScript camera => gameStateManager.GetCamera();

    protected Canvas stateCanvas;
    public bool isCurrentState { get; protected set; }

    public BaseState(GameStateManager manager)
    {
        this.gameStateManager = manager;
    }

    public virtual void Enter()
    {
        isCurrentState = true;
    }

    public virtual void Update() { }

    public virtual void Exit()
    {
        isCurrentState = false;
    }

    protected void SetCanvasActive(string canvasName, bool active)
    {
        var canvas = GameObject.Find(canvasName)?.GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.gameObject.SetActive(active);
        }
    }
}
