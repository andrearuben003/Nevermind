using System.Collections.Generic;
using UnityEngine;

namespace Nevermind.LevelSystem
{
    public class RoomManager : MonoBehaviour
    {
        public static RoomManager Instance;

        [SerializeField]
        private List<GameObject> rooms;

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
            rooms[0].SetActive(true);
            for (int i = 1; i < rooms.Count; i++)
                rooms[i].SetActive(false);
        }
        
        public int GetRoomIndex(GameObject room) => rooms.IndexOf(room);
        
        public void SwitchRoom(int index)
        {
            if (CurrentRoomIndex == index) return;
            
            Debug.Log($"Switching cameras {CurrentRoomIndex} --> {index}");
            
            rooms[CurrentRoomIndex].SetActive(false);
            rooms[index].SetActive(true);
            
            CurrentRoomIndex = index;
        }
    }
}