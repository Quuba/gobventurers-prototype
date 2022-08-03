using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms.Scripts
{
    public class DungeonRoomManager : MonoBehaviour
    {
        [SerializeField]
        private DungeonRoomData startRoom;

        [SerializeField]
        private List<DungeonRoomData> rooms;

        [SerializeField]
        private List<DungeonRoomData> corridors;

        private DungeonRoom currentRoom;

        // Start is called before the first frame update
        void Start()
        {
            GenerateStartRoom();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void GenerateStartRoom()
        {
            GameObject newStartRoom = Instantiate(startRoom.roomPrefab, Vector3.zero, Quaternion.identity);
            newStartRoom.transform.parent = transform;

            if (newStartRoom.TryGetComponent(out DungeonRoom room))
            {
                for (var i = 0; i < room.Passages.Count; i++)
                {
                }
            }
        }

        private void ConnectRooms(ref DungeonRoom room1, ref DungeonRoom room2)
        {
        }
    }

    class DungeonGenerator
    {
        private List<List<GridCell>> _grid;

        public void GenerateDungeon(int bossRoomDistance)
        {
            GenerateGrid(bossRoomDistance * 2 + 1, bossRoomDistance * 2 + 1
            );
            //Place start room in the middle
            _grid[bossRoomDistance][bossRoomDistance].Fill();
            //Place boss room at the circle with radius bossRoomDistance (North for now)
            _grid[bossRoomDistance][bossRoomDistance * 2].Fill();
        }

        private void GenerateGrid(int startHeight, int startWidth)
        {
            _grid = new List<List<GridCell>>();
            for (int i = 0; i < startWidth; i++)
            {
                _grid.Add(new List<GridCell>());
                for (int j = 0; j < startHeight; j++)
                {
                    _grid[i].Add(new GridCell(i, j));
                }
            }
        }

        private bool IsCellEmpty(int x, int y)
        {
            return _grid[x][y].isEmpty;
        }

        private bool AreCellsEmpty(int startX, int startY, int endX, int endY)
        {
            bool empty = true;
            for (int i = startX; i < endX; i++)
            {
                for (int j = startY; j < endY; j++)
                {
                    if (!_grid[i][j].isEmpty) empty = false;
                }
            }

            return empty;
        }


        struct GridCell
        {
            public int y;
            public int x;

            public bool isEmpty;

            public GridCell(int x, int y)
            {
                this.x = x;
                this.y = y;
                isEmpty = true;
            }

            public void Fill()
            {
                isEmpty = false;
            }
        }
    }
}