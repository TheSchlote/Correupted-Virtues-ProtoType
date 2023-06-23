using UnityEngine;

public class SelectableTile : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public Material normalMaterial;
    public Material highlightedMaterial;
    public TileMap map;
    public BattleController battleController;
    public GameObject characterOnTile;

    void OnMouseDown()
    {
        if (battleController.currentState is not BattlePlayerState)
        {
            return;
        }

        Character currentCharacter = battleController.currentCharacter.GetComponent<Character>();
        if (currentCharacter.isMoving)
        {
            return;
        }
        map.GeneratePathTo(tileX, tileY);
        currentCharacter.MoveToTile(tileX, tileY);
    }

    void OnMouseEnter()
    {
        if (battleController.currentState is not BattlePlayerState)
            return;

        Character currentCharacter = battleController.currentCharacter.GetComponent<Character>();
        if (currentCharacter.isMoving)
        {
            return;
        }
        map.GeneratePathTo(tileX, tileY);
        foreach (Node node in currentCharacter.currentPathList)
        {
            node.tile.ChangeMaterial(highlightedMaterial);
        }
    }

    void OnMouseExit()
    {
        if (battleController.currentState is not BattlePlayerState)
            return;

        Character currentCharacter = battleController.currentCharacter.GetComponent<Character>();
        if (currentCharacter.isMoving)
        {
            return;
        }
        foreach (Node node in currentCharacter.currentPathList)
        {
            node.tile.ChangeMaterial(normalMaterial);
        }
        currentCharacter.currentPathList = null;
    }


    public void ChangeMaterial(Material newMaterial)
    {
        GetComponent<MeshRenderer>().material = newMaterial;
    }

}
