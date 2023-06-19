public class BattleTransitionState : BattleBaseState
{
    public override void EnterState(BattleController battleController)
    {
        WhosNext(battleController);
    }
    public override void Update(BattleController battleController)
    {
        throw new System.NotImplementedException();
    }
    private void WhosNext(BattleController battleController)
    {
        battleController.characterTurnList.Sort((x, y) => y.GetComponent<Character>().speed.CompareTo(x.GetComponent<Character>().speed));
        battleController.currentCharacter = battleController.characterTurnList[0];

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
