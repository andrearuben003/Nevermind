using System.Collections.Generic;
using System.Linq;
using Nevermind.MemorySystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Nevermind.LevelSystem
{
    public class RespawnManager : MonoBehaviour
    {
        public static RespawnManager Instance;
        
        private int recordedCameraIndex = -1;
        private Vector3 recordedPosition;
        private List<Memory> recordedMemories;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += onSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= onSceneLoaded;
        }
        
        private void onSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //Bind to the reload input
            PlayerInput input = GameObject.Find("Input Manager").GetComponent<PlayerInput>();
            input.actions["Reload"].started += reload;
            
            if (recordedMemories == null) return;
            
            MemoryManager.Instance.transform.position = recordedPosition;
            MemoryManager.Instance.SetMemories(recordedMemories);
            RoomManager.Instance.SwitchRoom(recordedCameraIndex);
        }

        private void reload(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            Reload();
        }
        
        public void Reload()
        {
            Debug.Log("Reloading Scene");
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void RecordPlayer(Vector3 respawnPosition)
        {
            recordedCameraIndex = RoomManager.Instance.CurrentRoomIndex;
            recordedPosition = respawnPosition;
            recordedMemories = MemoryManager.Instance.GetMemories();
            
            Debug.Log($"Player recorded as: CamIndex: {recordedCameraIndex}, Position: {recordedPosition}, Memories: " +
                      $"[{recordedMemories.Select(m => m.ToString()).Aggregate("", (m1, m2) => m1 + ", " + m2)}]");
        }
    }
}