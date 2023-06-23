using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerState : BattleBaseState
{
    public override void EnterState(BattleController battleController)
    {
        
    }
    public override void Update(BattleController battleController)
    {
        QuickTimeEvents quickTimeEvents = battleController.currentCharacter.GetComponent<QuickTimeEvents>();

        // If the character is not attacking and player presses a key, start an attack.
        if (!quickTimeEvents.isAttacking && Input.GetKeyDown(KeyCode.Keypad1)) //Numpad 1
        {
            quickTimeEvents.Attack(QuickTimeEvents.QTEType.RapidPress);
            EnableCurrentCharacterUI(battleController);
        }
        else if (!quickTimeEvents.isAttacking && Input.GetKeyDown(KeyCode.Keypad2)) //Numpad 2
        {
            quickTimeEvents.Attack(QuickTimeEvents.QTEType.SinglePress);
            EnableCurrentCharacterUI(battleController);
        }
        else if (!quickTimeEvents.isAttacking && Input.GetKeyDown(KeyCode.Keypad3)) //Numpad 3
        {
            quickTimeEvents.Attack(QuickTimeEvents.QTEType.MatchPress);
            EnableCurrentCharacterUI(battleController);
        }

        // If the character was attacking and has finished, end the turn.
        if (quickTimeEvents.isAttacking && quickTimeEvents.quickTimeEventCompleted)
        {
            quickTimeEvents.isAttacking = false;
            quickTimeEvents.quickTimeEventCompleted = false;
            battleController.characterTurnList.Remove(battleController.currentCharacter);
            battleController.TransitionToState(battleController.transitionState);
        }
    }

    private void EnableCurrentCharacterUI(BattleController battleSystem)
    {
        GameObject uiElement = battleSystem.currentCharacter.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        uiElement.SetActive(true);
    }
}
