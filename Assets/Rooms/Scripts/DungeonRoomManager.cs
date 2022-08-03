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
                    // GameObject newRoomObj = Instantiate(corridors[0].roomPrefab);
                    // DungeonRoom newRoom = newRoomObj.GetComponent<DungeonRoom>();
                    // room.ConnectRoom(room.Passages[i], newRoom, newRoom.Passages[0]);
                    // Debug.Log(newRoom.Passages[0].connectedRoom);
                }
            }
        }
        
        private void ConnectRooms(ref DungeonRoom room1, ref DungeonRoom room2)
        {
            // if (passage.transform.forward == -newRoomPasage.transform.forward)
            // {
            //     var diffVec = newRoomPasage.GetPosition() - passage.GetPosition();
            //     newRoomPasage.transform.position += diffVec;
            // }
            // else
            // {
            //     Debug.Log("zoinks");
            //     //rotate and reposition
            //     //TODO: implement
            // }
            // passage.connectedRoom = newRoom;
            // newRoomPasage.connectedRoom = this;
        }
    }
}