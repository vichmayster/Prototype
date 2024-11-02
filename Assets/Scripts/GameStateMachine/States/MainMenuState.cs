using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuState : BaseState
{
    private Canvas mainMenuCanvas;
    private TextMeshProUGUI titleText;

    public MainMenuState(GameStateManager manager) : base(manager) { }

    public override void Enter()
    {
        Debug.Log("Entering Main Menu State");
        base.Enter();

        mainMenuCanvas = GameObject.Find("MainMenuCanvas")?.GetComponent<Canvas>();
        if (mainMenuCanvas != null)
        {
            titleText = mainMenuCanvas.transform.Find("MenuContainer/TitleText")?.GetComponent<TextMeshProUGUI>();
            if (titleText != null)
                titleText.text = "Runner";
            mainMenuCanvas.gameObject.SetActive(true);
        }

        var player = gameStateManager.GetPlayer();
        if (player != null)
            player.gameObject.SetActive(false);
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            gameStateManager.GoToPlaying();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Main Menu State");
        if (mainMenuCanvas != null)
            mainMenuCanvas.gameObject.SetActive(false);
    }
}
