using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public BattleBaseState currentState;
    public List<GameObject> characterTurnList;
    public GameObject currentCharacter;
    public TileMap tileMap;
    public List<GameObject> playerCharactesList;
    public List<GameObject> enemyCharactesList;
    
    public readonly BattleStartState startState = new BattleStartState();
    public readonly BattlePlayerState playerState = new BattlePlayerState();
    public readonly BattleEnemyState enemyState = new BattleEnemyState();
    public readonly BattleTransitionState transitionState = new BattleTransitionState();
    public readonly BattleEndState endState = new BattleEndState();
    private void Start()
    {
        TransitionToState(startState);
    }

    private void Update()
    {
        currentState.Update(this);
    }

    public void TransitionToState(BattleBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    //All things that have to do with Instantiate need to be under MonoBehavior
    public void SpawnPlayerCharacters()
    {
        for (int i = 0; i < playerCharactesList.Count; i++)
        {
            GameObject playerCharacter = Instantiate(playerCharactesList[i], new Vector3(i, 0, 0), Quaternion.identity);
            playerCharacter.GetComponent<Character>().map = tileMap;
            playerCharacter.GetComponent<Character>().currentNode = tileMap.nodeGrid[i, 0];
            playerCharacter.GetComponent<Character>().currentNode.occupyingCharacter = playerCharacter;
            characterTurnList.Add(playerCharacter);
        }
    }
    public void SpawnEnemyChracters()
    {
        for (int i = 0; i < enemyCharactesList.Count; i++)
        {
            GameObject enemyCharacter = Instantiate(enemyCharactesList[i], new Vector3(i, 0, tileMap.mapSizeY - 1), Quaternion.identity);
            enemyCharacter.GetComponent<Character>().map = tileMap;
            enemyCharacter.GetComponent<Character>().currentNode = tileMap.nodeGrid[i, tileMap.mapSizeY - 1];
            enemyCharacter.GetComponent<Character>().map = tileMap;
        }
    }


}
