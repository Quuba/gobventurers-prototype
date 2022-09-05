using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonCreator;
using Rooms.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private int cellSize = 2;
    [SerializeField] private int gridSize = 16;
    [SerializeField] private int distanceToBoss = 5;

    // [SerializeField] private int wfcIterations = 1;
    [SerializeField] private DungeonRoomData startRoom;

    [SerializeField] private List<DungeonRoomData> normalRooms;


    [SerializeField] private DungeonRoomData bossRoom;

    private Dictionary<Direction, List<DungeonRoomData>> normalRoomsByEntryDirection = new Dictionary<Direction, List<DungeonRoomData>>();

    private DungeonRoomData[,] _roomGrid;
    private GameObject[,] _roomPrefabGrid;

    // private wfcCell?[,] _wfcGrid;
    // private wfcCell?[,] _collapsedCells;

    // Start is called before the first frame update
    void Start()
    {
        _roomGrid = new DungeonRoomData[gridSize, gridSize];
        _roomPrefabGrid = new GameObject[gridSize, gridSize];

        // _wfcGrid = new wfcCell?[gridSize, gridSize];
        // _collapsedCells = new wfcCell?[gridSize, gridSize];


        foreach (var room in normalRooms)
        {
            if (room.exits.Contains(Direction.Bottom)) normalRoomsByEntryDirection[Direction.Top].Add(room);
            if (room.exits.Contains(Direction.Top)) normalRoomsByEntryDirection[Direction.Bottom].Add(room);
            if (room.exits.Contains(Direction.Right)) normalRoomsByEntryDirection[Direction.Left].Add(room);
            if (room.exits.Contains(Direction.Left)) normalRoomsByEntryDirection[Direction.Right].Add(room);
        }
    }

    public void GenerateDungeon()
    {
        ClearRooms();

        GenerateRoomGrid();


        PlaceRooms();
    }

    private void GenerateRoomGrid()
    {
        Vector2Int middlePoint = new Vector2Int(gridSize / 2, gridSize / 2);
        _roomGrid[middlePoint.x, middlePoint.y] = startRoom;


        Vector2 randomDir = Random.insideUnitCircle.normalized * distanceToBoss;
        Vector2Int bossRoomGridPos = new Vector2Int(middlePoint.x, middlePoint.y) +
                                     new Vector2Int(Mathf.RoundToInt(randomDir.x), Mathf.RoundToInt(randomDir.y));

        _roomGrid[bossRoomGridPos.x, bossRoomGridPos.y] = bossRoom;

        //first pass
        //connect start room to the boss room
        AStarNode startNode = new AStarNode(0, 0);
        switch (startRoom.exits[0])
        {
            case Direction.Top:
                startNode.x = middlePoint.x;
                startNode.y = middlePoint.y + 1;
                break;
            case Direction.Bottom:
                startNode.x = middlePoint.x;
                startNode.y = middlePoint.y - 1;
                break;
            case Direction.Left:
                startNode.x = middlePoint.x - 1;
                startNode.y = middlePoint.y;
                break;
            case Direction.Right:
                startNode.x = middlePoint.x + 1;
                startNode.y = middlePoint.y;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        AStarNode targetNode = new AStarNode(0, 0);
        switch (bossRoom.exits[0])
        {
            case Direction.Top:
                targetNode.x = bossRoomGridPos.x;
                targetNode.y = bossRoomGridPos.y + 1;
                break;
            case Direction.Bottom:
                targetNode.x = bossRoomGridPos.x;
                targetNode.y = bossRoomGridPos.y - 1;
                break;
            case Direction.Left:
                targetNode.x = bossRoomGridPos.x - 1;
                targetNode.y = bossRoomGridPos.y;
                break;
            case Direction.Right:
                targetNode.x = bossRoomGridPos.x + 1;
                targetNode.y = bossRoomGridPos.y;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }


        List<AStarNode> openList = new List<AStarNode>() {startNode};

        AStarNode[,] nodeGrid = new AStarNode[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                nodeGrid[i, j] = new AStarNode(i, j);
            }
        }

        List<AStarNode> closedList = new List<AStarNode>();


        bool isFinished = false;
        while (!isFinished)
        {
            AStarNode currentNode = FindLowestFCostNode(openList);
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.x == targetNode.x && currentNode.y == targetNode.y)
            {
                isFinished = true;
                break;
            }

            var topNode = nodeGrid[currentNode.x, currentNode.y + 1];
            if (!closedList.Contains(topNode))
            {
                
            }

            var bottomNode = nodeGrid[currentNode.x, currentNode.y - 1];
            var leftNode = nodeGrid[currentNode.x - 1, currentNode.y];
            var rightNode = nodeGrid[currentNode.x + 1, currentNode.y];


            isFinished = true;
        }
    }

    private AStarNode FindLowestFCostNode(List<AStarNode> nodeList)
    {
        AStarNode chosenNode = new AStarNode();
        int lowestCost = Int32.MaxValue;
        foreach (var node in nodeList)
        {
            if (node.f < lowestCost)
            {
                chosenNode = node;
            }
        }

        return chosenNode;
    }

    private void PlaceRooms()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (_roomGrid[i, j] != null)
                {
                    _roomPrefabGrid[i, j] = Instantiate(_roomGrid[i, j].roomPrefab, new Vector3(i * cellSize, 0, j * cellSize), Quaternion.identity);
                }
            }
        }
    }

    public void ClearRooms()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (_roomPrefabGrid[i, j] != null)
                {
                    _roomGrid[i, j] = null;
                    Destroy(_roomPrefabGrid[i, j].gameObject);
                    _roomPrefabGrid[i, j] = null; // not entirely sure if this is needed
                }
            }
        }
    }

    public struct AStarNode
    {
        public int x, y;

        public int f;
        public int g;
        public int h;

        public AStarNode(int x, int y) : this()
        {
            this.x = x;
            this.y = y;
            f = 0;
            g = 0;
            h = 0;
        }
    }
}

// public struct wfcCell
// {
//     public bool collapsed;
//     public int statesLeft;
//     public List<DungeonRoomData> remainingStates { get; set; }
//
//     // public int posX;
//     // public int posY;
//     public wfcCell(List<DungeonRoomData> states)
//     {
//         remainingStates = states;
//         statesLeft = states.Count;
//         // posX = x;
//         // posY = y;
//         collapsed = false;
//     }
//
//     public wfcCell(DungeonRoomData state)
//     {
//         remainingStates = new List<DungeonRoomData>() {state};
//         statesLeft = 1;
//         // posX = x;
//         // posY = y;
//         collapsed = true;
//     }
//
//     public void RemoveState(DungeonRoomData state)
//     {
//         remainingStates.Remove(state);
//         statesLeft--;
//
//         if (statesLeft == 1)
//         {
//             collapsed = true;
//         }
//     }
//
//     public void CollapseToState(DungeonRoomData state)
//     {
//         remainingStates = new List<DungeonRoomData>() {state};
//         statesLeft = 1;
//         collapsed = true;
//     }
//
//     // private void Collapse()
//     // {
//     //     collapsed = true;
//     // }
// }
