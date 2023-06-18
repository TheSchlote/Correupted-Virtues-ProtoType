using UnityEngine;

public class SelectableTile : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public Material normalMaterial;
    public Material highlightedMaterial;
    public TileMap map;
    public BattleSystem battleSystem;

    void OnMouseDown()
    {
        Character currentCharacter = battleSystem.currentCharacter.GetComponent<Character>();
        map.GeneratePathTo(tileX, tileY);
        currentCharacter.MoveToTile(tileX, tileY);
    }

    void OnMouseEnter()
    {
        Character currentCharacter = battleSystem.currentCharacter.GetComponent<Character>();
        map.GeneratePathTo(tileX, tileY);
        foreach (Node node in currentCharacter.currentPathList)
        {
            node.tile.ChangeMaterial(highlightedMaterial);
        }
    }

    void OnMouseExit()
    {
        Character currentCharacter = battleSystem.currentCharacter.GetComponent<Character>();
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
