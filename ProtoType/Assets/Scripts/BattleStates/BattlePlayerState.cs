using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerState : BattleBaseState
{
    public override void EnterState(BattleController battleSystem)
    {
        
    }
    public override void Update(BattleController battleSystem)
    {
        //this is where we will check when the player is pushing buttons
        if(Input.GetKeyDown(KeyCode.Space))
        {
            battleSystem.currentCharacter.GetComponent<QuickTimeEvents>().Attack();
            battleSystem.currentCharacter.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
