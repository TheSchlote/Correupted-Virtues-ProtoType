public class BattleTransitionState : BattleBaseState
{
    public override void EnterState(BattleController battleController)
    {
        WhosNext(battleController);
    }
    public override void Update(BattleController battleController)
    {
        //do nothing
    }
    private void WhosNext(BattleController battleController)
    {
        battleController.characterTurnList.Sort((x, y) => y.GetComponent<Character>().speed.CompareTo(x.GetComponent<Character>().speed));
        if (battleController.characterTurnList.Count > 0)
        {
            battleController.currentCharacter = battleController.characterTurnList[0];
        }
        else
        {
            //fill the list with characters again
            for (int i = 0; i < battleController.playerCharactesList.Count; i++)
            {
                battleController.characterTurnList.Add(battleController.playerCharactesList[i]);
            }
            for (int i = 0; i < battleController.enemyCharactesList.Count; i++)
            {
                battleController.characterTurnList.Add(battleController.enemyCharactesList[i]);
            }
        }
        if (battleController.currentCharacter.CompareTag("Player"))
        {
            battleController.TransitionToState(battleController.playerState);
        }
        else
        {
            battleController.TransitionToState(battleController.enemyState);
        }
    }
}
