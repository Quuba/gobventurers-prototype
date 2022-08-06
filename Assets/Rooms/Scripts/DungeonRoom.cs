using System;
using System.Collections.Generic;
using Shared.enums;
using UnityEngine;

namespace Rooms.Scripts
{
    public class DungeonRoom : MonoBehaviour
    {
        [SerializeField]
        private List<Passage> passages;

        public List<Passage> Passages => passages;
        [SerializeField] private DungeonRoomType roomType;
        public DungeonRoomType RoomType => roomType;

        
    }

    [Serializable]
    public struct Passage
    {
        public Transform transform;
        public DungeonRoom connectedRoom;

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}