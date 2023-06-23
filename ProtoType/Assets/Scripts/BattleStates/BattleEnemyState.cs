using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemyState : BattleBaseState
{
    public override void EnterState(BattleController battleController)
    {
        // Find closest player character
        Character closestPlayer = battleController.tileMap.FindClosestCharacter("Player", battleController.currentCharacter.transform.position);

        // Find a tile next to the player
        Node playerNode = battleController.tileMap.nodeGrid[closestPlayer.tileX, closestPlayer.tileY];
        Node nextNode = battleController.tileMap.GetClosestAdjacentNode(playerNode);

        // Start the enemy turn sequence
        battleController.StartCoroutine(EnemyTurn(battleController, closestPlayer, nextNode));
    }

    private IEnumerator EnemyTurn(BattleController battleController, Character target, Node moveTo)
    {
        Character enemy = battleController.currentCharacter.GetComponent<Character>();

        // Command the enemy to move to the tile
        enemy.MoveToTile(moveTo.x, moveTo.y);

        // Wait until movement has finished
        yield return new WaitUntil(() => enemy.isMoving == false);

        // Then attack the player
        enemy.Attack(target);

        // And finally, end the turn
        battleController.characterTurnList.Remove(battleController.currentCharacter);
        battleController.TransitionToState(battleController.transitionState);
    }



    public override void Update(BattleController battleSystem)
    {
        //do nothing
    }
}
