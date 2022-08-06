using System.Collections.Generic;
using Rooms.Scripts;
using UnityEngine;

namespace DungeonCreator
{
    [CreateAssetMenu(fileName = "DungeonData", menuName = "Scriptable Objects/Dungeon")]
    public class DungeonData : ScriptableObject
    {
        public List<List<DungeonRoomData>> DungeonRoomGrid;
    }
}