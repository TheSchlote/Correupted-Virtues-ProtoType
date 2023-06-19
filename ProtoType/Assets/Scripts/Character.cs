using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public Node currentNode;
    public TileMap map;
    public bool isMoving;
    public List<Node> currentPathList;

    public int maxHealth = 10;
    public int currentHealth = 10;
    public int attackPower = 2;
    public int speed = 5;

    public void MoveToTile(int destX, int destY)
    {
        map.GeneratePathTo(destX, destY);

        // Now we need to move the character along the path
        if (currentPathList != null && currentPathList.Count > 0)
        {
            StartCoroutine(MoveAlongPath(currentPathList));
        }
    }

    IEnumerator MoveAlongPath(List<Node> path)
    {
        isMoving = true;
        foreach (Node node in path)
        {
            while (new Vector3(node.x, 0, node.y) != transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(node.x, 0, node.y), Time.deltaTime);
                yield return null;
            }
            tileX = node.x;
            tileY = node.y;
        }
        isMoving = false;
    }

    public void Attack(Character other)
    {
        other.currentHealth -= attackPower;
    }
}
