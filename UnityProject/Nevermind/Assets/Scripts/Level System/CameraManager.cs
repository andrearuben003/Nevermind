using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Nevermind.LevelSystem
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;

        [SerializeField]
        private List<CinemachineCamera> roomCameras;

        private int activeCameraIndex;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        
        public int GetActiveCameraIndex() => activeCameraIndex;
        
        public void SwitchCamera(int index)
        {
            if (activeCameraIndex == index) return;
            
            Debug.Log($"Switching cameras {activeCameraIndex} --> {index}");
            
            roomCameras[activeCameraIndex].gameObject.SetActive(false);
            roomCameras[index].gameObject.SetActive(true);
            
            activeCameraIndex = index;
        }
    }
}