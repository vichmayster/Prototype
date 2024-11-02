using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LevelCompleteState", menuName = "Runner Game/States/Level Complete State")]
public class LevelCompleteStateScriptable : GameStateScriptable
{
    public override void OnStateEnter()
    {
        Debug.Log("Entering Level Complete State");
        base.OnStateEnter();

        if (stateCanvas == null)
        {
            stateCanvas = GameObject.Find("GameOverCanvas")?.GetComponent<Canvas>();
        }

        if (stateCanvas != null)
        {
            stateCanvas.gameObject.SetActive(true);
            Debug.Log($"{stateCanvas.name} activated");
        }
        else
        {
            Debug.LogError($"{stateCanvas.name} not found!");
        }
    }

    public override void OnStateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.N)) // Next level
        {
            Time.timeScale = 1f;
            GameStateManager.Instance.GoToPlaying();
        }
        else if (Input.GetKeyDown(KeyCode.M)) // Main menu
        {
            Time.timeScale = 1f;
            GameStateManager.Instance.GoToMainMenu();
        }
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Level Complete State");
        base.OnStateExit();

        Time.timeScale = 1f;
    }
}