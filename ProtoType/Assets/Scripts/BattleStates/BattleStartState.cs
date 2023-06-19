public class BattleStartState : BattleBaseState
{
    public override void EnterState(BattleController battleSystem)
    {
        battleSystem.tileMap.GererateTileGridData();
        battleSystem.tileMap.GenerateNodeGridPathfinding();
        battleSystem.tileMap.GenerateVisualRepresentationOfMap();
        //eventually i want to pass in their starting positions too but for now theyre fixed
        battleSystem.SpawnPlayerCharacters();
        battleSystem.SpawnEnemyChracters();
        battleSystem.TransitionToState(battleSystem.transitionState);
    }

    public override void Update(BattleController battleSystem)
    {
        
    }
}
