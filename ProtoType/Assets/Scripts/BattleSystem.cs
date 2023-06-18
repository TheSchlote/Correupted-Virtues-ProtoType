using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public enum State
    {
        Start,
        PlayerTurn,
        EnemyTurn,
        SwitchTurn,
        Won,
        Lost,
    }

    public State state;
    public List<GameObject> characterTurnList;
    public GameObject currentCharacter;
    public TileMap tileMap;
    public List<GameObject> playerCharactesList;
    public List<GameObject> enemyCharactesList;

    private void Start()
    {
        state = State.Start;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Start:
                // Handle game start
                SpawnPlayerCharacters();
                SpawnEnemyChracters();
                FillCharacterTurnList();
                state = State.SwitchTurn;
                break;

            case State.PlayerTurn:
                // Handle player's turn
                break;

            case State.EnemyTurn:
                // Handle enemy's turn
                break;

            case State.SwitchTurn:
                WhosNext();
                // Handle turn switch
                break;

            case State.Won:
                // Handle victory
                break;

            case State.Lost:
                // Handle defeat
                break;
        }
    }

    private void WhosNext()
    {
        characterTurnList.Sort((x, y) => y.GetComponent<Character>().speed.CompareTo(x.GetComponent<Character>().speed));
        currentCharacter = characterTurnList[0];
        state = currentCharacter.CompareTag("Player") ? State.PlayerTurn : State.EnemyTurn;
    }

    private void FillCharacterTurnList()
    {
        // Fill characterTurnList with all characters
        for (int i = 0; i < playerCharactesList.Count; i++)
        {
            characterTurnList.Add(playerCharactesList[i]);
        }
        for (int i = 0; i < enemyCharactesList.Count; i++)
        {
            characterTurnList.Add(enemyCharactesList[i]);
        }
    }

    void SpawnPlayerCharacters()
    {
        for (int i = 0; i < playerCharactesList.Count; i++)
        {
            GameObject playerCharacter = Instantiate(playerCharactesList[i], new Vector3(i, 0, 0), Quaternion.identity);
            playerCharacter.GetComponent<Character>().map = tileMap;
            characterTurnList.Add(playerCharacter);
        }
    }
    void SpawnEnemyChracters()
    {
        for (int i = 0; i < enemyCharactesList.Count; i++)
        {
            GameObject enemyCharacter = Instantiate(enemyCharactesList[i], new Vector3(i, 0, tileMap.mapSizeY - 1), Quaternion.identity);
            enemyCharacter.GetComponent<Character>().map = tileMap;
        }
    }
}
