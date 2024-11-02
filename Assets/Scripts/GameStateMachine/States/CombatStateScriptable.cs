using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatState", menuName = "Runner Game/States/Combat State")]
public class CombatStateScriptable : GameStateScriptable
{
    public override void Initialize(PlayerMovement playerRef, CameraScript cameraRef)
    {
        base.Initialize(playerRef, cameraRef);
        stateName = "Combat";
    }

    public override void OnStateEnter()
    {
        Debug.Log("Entering Combat State");
        base.OnStateEnter();

        if (player != null)
        {
            var combatScript = player.GetComponent<PlayerCombatScript>();
            if (combatScript != null)
            {
                combatScript.enabled = true;
            }
        }
    }


    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameStateManager.Instance.GoToPaused();
        }
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Combat State");
        base.OnStateExit();
    }
}