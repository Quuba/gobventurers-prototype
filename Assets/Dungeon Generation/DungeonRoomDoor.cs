using System;
using UnityEngine;

namespace Dungeon_Generation
{
    public class DungeonRoomDoor : MonoBehaviour
    {
        public event Action onEntered;
        public bool isLocked = false;
        public DungeonRoom room;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isLocked)
            {
                onEntered?.Invoke();
            }
        }
    }
}
