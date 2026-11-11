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

        private void Start()
        {
            roomCameras[0].gameObject.SetActive(true);
            for (int i = 1; i < roomCameras.Count; i++)
                roomCameras[i].gameObject.SetActive(false);
        }

        public int GetActiveCameraIndex() => activeCameraIndex;

        public int GetCameraIndex(CinemachineCamera cam) => roomCameras.IndexOf(cam);
        
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