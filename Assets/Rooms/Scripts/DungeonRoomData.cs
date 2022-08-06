using System;
using System.Collections.Generic;
using Shared.enums;
using UnityEngine;

namespace Rooms.Scripts
{
    [CreateAssetMenu(fileName = "DungeonRoomData", menuName = "Scriptable Objects/Dungeon Room")]
    public class DungeonRoomData : ScriptableObject
    {
        public GameObject roomPrefab;

        public List<Direction> exits;

        // public int passageCount;
        // public RoomShape roomShape;
        public List<EnemySpawn> enemySpawns;

        [Serializable]
        public struct EnemySpawn
        {
            //Todo: add
            // public EnemyData data;
            public Vector3 spawnPos;
        }
    }

    public enum Direction
    {
        Top,
        Bottom,
        Left,
        Right
    }
}