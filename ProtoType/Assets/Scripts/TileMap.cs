using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public BattleController battleController;
    public TileType[] tileTypes;
    int[,] tileGrid;
    public Node[,] nodeGrid;
    public readonly int mapSizeX = 5;
    public readonly int mapSizeY = 5;

    private void Start()
    {

    }
    public void GererateTileGridData()
    {
        tileGrid = new int[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                tileGrid[x, y] = 0;
            }
        }
    }
    public void GenerateNodeGridPathfinding()
    {
        nodeGrid = new Node[mapSizeX, mapSizeY];
        //Initialize a Node for each spot in the array
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                nodeGrid[x, y] = new Node
                {
                    x = x,
                    y = y
                };
            }
        }
        //Now that all the nodes exist, calculate their neighbours
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                //For Square map
                if (x > 0)
                {
                    nodeGrid[x, y].neighbours.Add(nodeGrid[x - 1, y]);
                }
                if (x < mapSizeX - 1)
                {
                    nodeGrid[x, y].neighbours.Add(nodeGrid[x + 1, y]);
                }
                if (y > 0)
                {
                    nodeGrid[x, y].neighbours.Add(nodeGrid[x, y - 1]);
                }
                if (y < mapSizeY - 1)
                {
                    nodeGrid[x, y].neighbours.Add(nodeGrid[x, y + 1]);
                }
            }
        }
    }
    public void GenerateVisualRepresentationOfMap()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                TileType currentTileType = tileTypes[tileGrid[x, y]];
                GameObject newTileObject = Instantiate(currentTileType.tileVisualPrefab, new Vector3(x, 0, y), Quaternion.identity);
                newTileObject.transform.parent = transform;
                newTileObject.name = $"Tile_{x}_{y}";
                SelectableTile selectableTile = newTileObject.GetComponent<SelectableTile>();
                selectableTile.tileX = x;
                selectableTile.tileY = y;
                selectableTile.map = this;
                selectableTile.battleController = battleController;
                nodeGrid[x, y].tile = selectableTile;
            }
        }
    }
    public void GeneratePathTo(int destinationX, int destinationY)
    {
        //Clear out our unit's old path.
        Character selectedUnit = battleController.currentCharacter.GetComponent<Character>();
        selectedUnit.currentPathList = null;

        Dictionary<Node, float> nodeDistanceMap = new();
        Node sourceNode = nodeGrid[selectedUnit.tileX, selectedUnit.tileY];
        nodeDistanceMap[sourceNode] = 0;
        Dictionary<Node, Node> previousNodeMap = new()
        {
            [sourceNode] = null
        };

        List<Node> unvisitedNodes = new();
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                Node currentNode = nodeGrid[x, y];
                if (currentNode != sourceNode)
                {
                    nodeDistanceMap[currentNode] = Mathf.Infinity;
                    previousNodeMap[currentNode] = null;
                }
                unvisitedNodes.Add(currentNode);
            }
        }
        Node targetNode = nodeGrid[destinationX, destinationY];
        while (unvisitedNodes.Count > 0)
        {
            Node shortestDistanceNode = null;
            foreach (Node possibleNode in unvisitedNodes)
            {
                if (shortestDistanceNode == null || nodeDistanceMap[possibleNode] < nodeDistanceMap[shortestDistanceNode])
                {
                    shortestDistanceNode = possibleNode;
                }
            }

            if (shortestDistanceNode == targetNode)
            {
                break;
            }

            unvisitedNodes.Remove(shortestDistanceNode);

            foreach (Node neighbourNode in shortestDistanceNode.neighbours)
            {
                float alternatePathDistance = nodeDistanceMap[shortestDistanceNode] + shortestDistanceNode.DistanceTo(neighbourNode);

                if (alternatePathDistance < nodeDistanceMap[neighbourNode])
                {
                    nodeDistanceMap[neighbourNode] = alternatePathDistance;
                    previousNodeMap[neighbourNode] = shortestDistanceNode;
                }
            }
        }
        if (previousNodeMap[targetNode] == null)
        {
            //No route between target and source
            return;
        }
        List<Node> currentPath = new();
        Node curr = targetNode;

        while (curr != null)
        {
            currentPath.Add(curr);
            curr = previousNodeMap[curr];
        }
        currentPath.Reverse();

        selectedUnit.currentPathList = currentPath;
    }
    public Character FindClosestCharacter(string tag, Vector3 position)
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject character in characters)
        {
            Vector3 direction = character.transform.position - position;
            float distance = direction.sqrMagnitude;
            if (distance < closestDistance)
            {
                closest = character;
                closestDistance = distance;
            }
        }

        return closest.GetComponent<Character>();
    }
    public Node GetClosestAdjacentNode(Node centerNode)
    {
        Node closestNode = null;
        float closestDistance = Mathf.Infinity;

        foreach (Node neighbour in centerNode.neighbours)
        {
            // Check if the tile is not occupied by a Character
            if (!neighbour.tile.GetComponent<SelectableTile>().characterOnTile)
            {
                float distance = Vector3.Distance(centerNode.tile.transform.position, neighbour.tile.transform.position);
                if (distance < closestDistance)
                {
                    closestNode = neighbour;
                    closestDistance = distance;
                }
            }
        }

        return closestNode;
    }


}