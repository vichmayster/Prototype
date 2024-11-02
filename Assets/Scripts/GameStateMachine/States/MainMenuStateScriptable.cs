using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "MainMenuState", menuName = "Runner Game/States/Main Menu State")]
public class MainMenuStateScriptable : GameStateScriptable
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI titleText;

    public override void OnStateEnter()
    {
        Debug.Log("Entering Main Menu State Scriptable");
        base.OnStateEnter();

        if (stateCanvas == null)
        {
            stateCanvas = GameObject.Find("MainMenuCanvas")?.GetComponent<Canvas>();
        }

        if (stateCanvas != null)
        {
            if (titleText == null)
            {
                titleText = stateCanvas.transform.Find("MenuContainer/TitleText")?.GetComponent<TextMeshProUGUI>();
            }

            if (titleText != null)
            {
                titleText.text = "Runner";
            }

            stateCanvas.gameObject.SetActive(true);
        }

        if (player != null)
        {
            player.gameObject.SetActive(false);
        }

        Time.timeScale = 0f;
    }

    public override void OnStateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return) && GameStateManager.Instance != null)
        {
            GameStateManager.Instance.GoToPlaying();
        }
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Main Menu State Scriptable");
        base.OnStateExit();
        Time.timeScale = 1f;
    }
}