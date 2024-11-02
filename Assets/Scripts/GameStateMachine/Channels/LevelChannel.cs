using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "LevelChannel", menuName = "Runner Game/Channels/Level Channel")]
public class LevelChannel : ScriptableObject
{
    public UnityEvent<int> OnLevelComplete;
    public UnityEvent OnLoadNextLevel;

    private void OnEnable()
    {
        OnLevelComplete ??= new UnityEvent<int>();
        OnLoadNextLevel ??= new UnityEvent();
    }
}