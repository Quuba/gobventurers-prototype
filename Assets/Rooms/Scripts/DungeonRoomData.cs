using System.Collections.Generic;
using UnityEngine;

namespace Rooms.Scripts
{
    [CreateAssetMenu(fileName = "DungeonRoomData", menuName = "Scriptable Objects/Dungeon Room")]
    public class DungeonRoomData : ScriptableObject
    {
        public GameObject roomPrefab;
    }
}
