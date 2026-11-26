using System.Collections.Generic;
using UnityEngine;

namespace Nevermind.LevelSystem
{
    public class RoomManager : MonoBehaviour
    {
        public static RoomManager Instance;

        [SerializeField]
        private List<Room> rooms;

        public int CurrentRoomIndex { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == CurrentRoomIndex) continue;
                rooms[i].DisableRoom();
            }
        }
        
        public int GetRoomIndex(Room room) => rooms.IndexOf(room);
        
        public void SwitchRoom(int index)
        {
            if (CurrentRoomIndex == index) return;
            
            Debug.Log($"Switching cameras {CurrentRoomIndex} --> {index}");
            
            rooms[CurrentRoomIndex].DisableRoom();
            rooms[index].EnableRoom();
            
            CurrentRoomIndex = index;
        }
    }
}