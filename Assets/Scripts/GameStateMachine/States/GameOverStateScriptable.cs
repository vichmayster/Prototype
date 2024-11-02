using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameOverState", menuName = "Runner Game/States/Game Over State")]
public class GameOverStateScriptable : GameStateScriptable
{
    public override void OnStateEnter()
    {
        Debug.Log("Entering Game Over State");
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameStateManager.Instance.GoToPlaying();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            GameStateManager.Instance.GoToMainMenu();
        }
    }
}