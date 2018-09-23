using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
    //Node collection data
    [HideInInspector]
    public Dictionary<Vector2, Vector2> keyCameFromValueMap;
    [SerializeField]
    private Transform gridContainer;
    [SerializeField]
    private Transform highCostTileContainer;
    [SerializeField]
    private Transform source;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private List<Vector2> neighborDirection;
    private Vector2 currentNode;
    private List<Vector2> neighborsOfCurrentNode;
    private HashSet<Vector2> walkableTilesHashSet;
    private HashSet<Vector2> highCostTilesHashSet;

    //Debug data
    [SerializeField]
    private bool displayDebugTiles = false;
    [SerializeField]
    private Transform debugSprite;
    private TileDebugger debugInstance;

    ///<summary>
    ///<para>Maximum number of iterations per frame</para>
    ///</summary>
    [SerializeField]
    private int maxNodeIterations = 50;

    private int newCost;
    private Dictionary<Vector2, int> nodeCostMap;
    private HashSet<Vector2> costSoFarHash;

    //Outer most list contains the cost and the sub-list contains the index of the prioritized node from priorityNodes
    private List<List<int>> priorityNodeIndexMap;
    private List<Vector2> priorityNodes;

    public bool isPathfindingComplete = false;

    private void ObjectInitializer()
    {
        walkableTilesHashSet = new HashSet<Vector2>();
        highCostTilesHashSet = new HashSet<Vector2>();
        nodeCostMap = new Dictionary<Vector2, int>();
        costSoFarHash = new HashSet<Vector2>();
        priorityNodeIndexMap = new List<List<int>>();
        priorityNodes = new List<Vector2>();
        keyCameFromValueMap = new Dictionary<Vector2, Vector2>();
    }

    private void IncludeDebugUtility()
    {
        if (displayDebugTiles)
        {
            if (GetComponent<TileDebugger>() == null)
            {
                debugInstance = gameObject.AddComponent<TileDebugger>();
                debugInstance.debugTile = debugSprite;
            }

            debugInstance.Initializer(source);
        }
    }

    private void InitializeCollections()
    {
        keyCameFromValueMap[source.position] = source.position;
        priorityNodeIndexMap.Add(new List<int>());
        priorityNodeIndexMap[0].Add(0);
        priorityNodes.Add(source.position);
        nodeCostMap[source.position] = 0;
        costSoFarHash.Add(source.position);
        currentNode = source.position;
    }

    private void GetTileCollection()
    {
        for (int i = 0; i < gridContainer.childCount; ++i)
        {
            walkableTilesHashSet.Add(gridContainer.GetChild(i).position);
        }
    }

    private List<Vector2> Neighbors(Vector2 current)
    {
        List<Vector2> neighborsCollection = new List<Vector2>();
        Vector2 checkNeighbor;

        for (int i = 0; i < neighborDirection.Count; ++i)
        {
            checkNeighbor = current + neighborDirection[i];

            if (walkableTilesHashSet.Contains(checkNeighbor) || highCostTilesHashSet.Contains(checkNeighbor))
            {
                neighborsCollection.Add(checkNeighbor);
            }
        }

        return neighborsCollection;
    }

    private void SetHighCostTiles()
    {
        for (int i = 0; i < highCostTileContainer.childCount; ++i)
        {
            highCostTilesHashSet.Add(highCostTileContainer.GetChild(i).position);
        }
    }

    private int Cost(Vector2 neighboringNode)
    {
        if (highCostTilesHashSet.Contains(neighboringNode))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private void GetBestPriorityTile()
    {
        for (int j = 0; j < priorityNodeIndexMap.Count; j++)
        {
            if (priorityNodeIndexMap[j].Count > 0)
            {
                currentNode = priorityNodes[priorityNodeIndexMap[j][0]];
                priorityNodeIndexMap[j].Remove(priorityNodeIndexMap[j][0]);

                break;
            }
        }
    }

    private void DetermineCostOfNeighboringNodes()
    {
        neighborsOfCurrentNode = Neighbors(currentNode);

        for (int j = 0; j < neighborsOfCurrentNode.Count; ++j)
        {
            newCost = nodeCostMap[currentNode] + Cost(neighborsOfCurrentNode[j]);

            if (!costSoFarHash.Contains(neighborsOfCurrentNode[j]) || newCost < nodeCostMap[currentNode])
            {
                nodeCostMap[neighborsOfCurrentNode[j]] = newCost;

                if (!costSoFarHash.Contains(neighborsOfCurrentNode[j]))
                {
                    costSoFarHash.Add(neighborsOfCurrentNode[j]);
                }

                PrioritizeCostOfNewTile(newCost, neighborsOfCurrentNode[j]);

                if (displayDebugTiles)
                {
                    //currentNodeCollection maps to cameFromCollection. This is only used for Debug purposes
                    debugInstance.currentNodeCollection.Add(currentNode);
                    debugInstance.cameFromCollection.Add(neighborsOfCurrentNode[j]);
                }

                //This is used by agent to reconstruct the path
                keyCameFromValueMap[neighborsOfCurrentNode[j]] = currentNode;
            }
        }
    }

    public void RestartSearch()
    {
        isPathfindingComplete = false;

        priorityNodes.Clear();
        priorityNodeIndexMap.Clear();
        neighborsOfCurrentNode.Clear();
        nodeCostMap.Clear();
        costSoFarHash.Clear();
        keyCameFromValueMap.Clear();
        highCostTilesHashSet.Clear();
        SetHighCostTiles();

        if (displayDebugTiles)
        {
            debugInstance.Reinitializer(source);
        }

        InitializeCollections();

        if (displayDebugTiles)
        {
            debugInstance.RemoveDebugTiles();
        }
    }

    private void PrioritizeCostOfNewTile(int cost, Vector2 neighbor)
    {
        if (cost > priorityNodeIndexMap.Count - 1)
        {
            //For loop inserts priority on the outer most list and the corresponding Vector value in the sub-list
            for (int k = priorityNodeIndexMap.Count; k <= (cost + 1); ++k)
            {
                if ((k - 1) == cost)
                {
                    priorityNodes.Add(neighbor);
                    priorityNodeIndexMap.Add(new List<int>());
                    priorityNodeIndexMap[k].Add(priorityNodes.Count - 1);
                }
                else
                {
                    priorityNodeIndexMap.Add(new List<int>());
                }
            }
        }
        else
        {
            //Problem: if we remove a node and its reference from line 227 and 228, then we do not have the same reference (SOLVED)
            //PN:       [0] = Vector [1] = Vector [2] = Vector [3] = Vector [4] = Vector [5] = Vector
            //PQ[0]:    [0] = 0 [1] = 1 [2] = 2 [3] = 3 [4] = 4 [5] = 5                                 => Each element corresponds to the relevant element in PN
            //When [0] gets removed on both PN and PQ[0], PQ[0][0] = 1 will now refer to PN[2]
            priorityNodes.Add(neighbor);
            priorityNodeIndexMap[cost].Add(priorityNodes.Count - 1);
        }
    }

    void Start()
    {
        ObjectInitializer();

        IncludeDebugUtility();

        InitializeCollections();

        GetTileCollection();

        SetHighCostTiles();
    }

    void Update()
    {
        if (priorityNodes.Count > 0 && !isPathfindingComplete)
        {
            for (int i = 0; i < maxNodeIterations; i++)
            {
                //Prevents this object from searching further, in order to save CPU. If there are multiple sources (enemies) this will be ignored
                if (currentNode == new Vector2(target.position.x, target.position.y))
                {
                    isPathfindingComplete = true;
                    break;
                }

                GetBestPriorityTile();

                DetermineCostOfNeighboringNodes();
            }
        }

        //Debug Start
        if (displayDebugTiles)
        {
            debugInstance.ShowDebugTiles();
        }
        //Debug End
    }
}
