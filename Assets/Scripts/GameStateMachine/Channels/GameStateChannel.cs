using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "GameStateChannel", menuName = "Runner Game/Channels/Game State Channel")]
public class GameStateChannel : ScriptableObject
{
    public Action<GameStateScriptable> StateEnter;
    public Action<GameStateScriptable> StateExit;
    public Func<GameStateScriptable> GetCurrentState;

    public void StateEntered(GameStateScriptable gameState)
    {
        StateEnter?.Invoke(gameState);
    }

    public void StateExited(GameStateScriptable gameState)
    {
        StateExit?.Invoke(gameState);
    }
}