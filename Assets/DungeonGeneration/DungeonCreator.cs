using System.Collections.Generic;
using System.Linq;
using Rooms.Scripts;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DungeonCreator
{
    public class DungeonCreator : MonoBehaviour
    {
        [SerializeField] private DungeonData activeDungeon;
        [SerializeField] private ActionMode currentActionMode;
        [SerializeField] private DungeonRoomData currentRoom;
        [SerializeField] private int cellSize;
        private int gridSize = 16;

        private DungeonRoomData[,] _roomGrid;
        private GameObject[,] _roomPrefabGrid;

        // Start is called before the first frame update
        void Start()
        {
            _roomGrid = new DungeonRoomData[gridSize, gridSize];
            _roomPrefabGrid = new GameObject[gridSize, gridSize];
            // for (int i = 0; i < gridSize; i++)
            // {
            //     // _roomGrid[i] = new DungeonRoomData[gridSize];
            //     // _roomPrefabGrid[i] = new GameObject[gridSize];
            //     // for (int j = 0; j < gridSize; j++)
            //     // {
            //     //     _roomGrid[i].Add(null);
            //     //     _roomPrefabGrid.Add(null);
            //     // }
            // }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var mouseGridPos = GetMouseGridPos();
                if(IsMouseOverUI()) return;

                PlaceRoom(currentRoom, mouseGridPos.x, mouseGridPos.z);
            }
        }

        void PlaceRoom(DungeonRoomData room, int gridX, int gridY)
        {
            if (gridX < 0 || gridY < 0)
            {
                Debug.LogError("ERROR: room coordinates cannot be negative");
                return;
            }

            _roomGrid[gridX, gridY] = room;
            // GameObject newRoom = 
            // newRoom.transform.parent = transform;
            _roomPrefabGrid[gridX, gridY] = Instantiate(room.roomPrefab, new Vector3(gridX, 0, gridY) * cellSize, Quaternion.identity);
        }

        // private Vector3Int GetAlignedMousePos()
        // {
        //     var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     var alignedWorldPos = new Vector3Int(Mathf.RoundToInt(worldPos.x / cellSize) * cellSize, 0, Mathf.RoundToInt(worldPos.z / cellSize) * cellSize);
        //     return alignedWorldPos;
        // }

        private Vector3Int GetMouseGridPos()
        {
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var gridPos = new Vector3Int(Mathf.RoundToInt(worldPos.x / cellSize), 0, Mathf.RoundToInt(worldPos.z / cellSize));
            return gridPos;
        }

        public void SaveDungeon()
        {
            List<List<DungeonRoomData>> dungeonRoomList = new List<List<DungeonRoomData>>();
            for (int i = 0; i < gridSize; i++)
            {
                var newList = new List<DungeonRoomData>();
                for (int j = 0; j < gridSize; j++)
                {
                    newList.Add(_roomGrid[i, j]);
                }

                dungeonRoomList.Add(newList);
            }

            activeDungeon.DungeonRoomGrid = dungeonRoomList;
            Debug.Log("saved a dungeon");
        }

        // public void LoadDungeon(DungeonData data)
        // {
        //     _roomGrid = data.DungeonRoomGrid;
        // }

        public void LoadDungeon()
        {
            ClearRooms();
            
            //TODO: gridSize = newDungeon.gridSize
            _roomGrid = new DungeonRoomData[gridSize, gridSize];
            _roomPrefabGrid = new GameObject[gridSize, gridSize];


            var roomList = activeDungeon.DungeonRoomGrid;

            for (int i = 0; i < roomList.Count; i++)
            {
                var column = roomList[i];

                for (int j = 0; j < roomList.Count; j++)
                {
                    _roomGrid[i, j] = column[j];
                    if (column[j] != null)
                    {
                        _roomPrefabGrid[i, j] = Instantiate(column[j].roomPrefab, new Vector3(i, 0, j) * cellSize, Quaternion.identity);
                    }
                }
            }

            Debug.Log("loaded a dungeon");
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
                        _roomPrefabGrid[i, j] = null;
                    }
                }
            }
        }

        private bool IsMouseOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        enum ActionMode
        {
            Place,
            Delete,
            Select
        }
    }
}