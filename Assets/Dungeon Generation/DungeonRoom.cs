using System;
using System.Collections.Generic;
using Shared.enums;
using UnityEngine;

namespace Dungeon_Generation
{
    public class DungeonRoom : MonoBehaviour
    {
        [SerializeField] private List<DoorEntity> doorList = new List<DoorEntity>();

        private void OnEnable()
        {
            foreach (DoorEntity doorEntity in doorList)
            {
                doorEntity.door.onEntered += () => HandleDoorEntered(doorEntity);
                doorEntity.door.room = this;
            }
        }

        void HandleDoorEntered(DoorEntity enteredDoorEntity)
        {
            Debug.Log($"Player entered a {enteredDoorEntity.exitDirection.ToString()} door");
            DungeonManager.Instance.ChangeRoom(enteredDoorEntity.connectedRoom, enteredDoorEntity.connectedDoor);
        }

        [Serializable]
        struct DoorEntity
        {
            public DungeonRoomDoor door;
            public Direction exitDirection;
            public DungeonRoom connectedRoom;
            public DungeonRoomDoor connectedDoor;
        }

        
        
    }
}