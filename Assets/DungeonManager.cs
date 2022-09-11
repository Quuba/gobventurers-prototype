using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Dungeon_Generation;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;
    [SerializeField] private DungeonRoom currentRoom;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeRoom(DungeonRoom nextRoom, DungeonRoomDoor entryDoor)
    {
        currentRoom = nextRoom;
        Entities.Player.Player player = GameManager.Instance.Player;
        entryDoor.isLocked = true;
        float playerY = player.transform.position.y;
        Vector3 newPos = entryDoor.transform.position;
        newPos.y = playerY;
        player.transform.position = newPos;
        
    }
}
