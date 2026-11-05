using Nevermind.LevelSystem;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraManager))]
public class CameraManagerInspector : Editor
{
    private Transform camerasHeader;
    private Transform levelHeader;

    public override void OnInspectorGUI()
    {
        if (!camerasHeader | !levelHeader)
        {
            camerasHeader = GameObject.Find("---- CAMERAS ----").transform;
            levelHeader = GameObject.Find("---- LEVEL MAP ----").transform;
        }
        
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Add Room"))
            addRoom();
    }

    private void addRoom()
    {
        SerializedProperty cameras = serializedObject.FindProperty("roomCameras");
        
        // Add new camera
        GameObject newCam = new GameObject();
        newCam.transform.parent = camerasHeader;
        newCam.name = $"Camera {cameras.arraySize}";
        newCam.transform.position = Vector3.back * 10;
        CinemachineCamera camera = newCam.AddComponent<CinemachineCamera>();
        camera.Lens.OrthographicSize = 5f;
        
        cameras.InsertArrayElementAtIndex(cameras.arraySize);
        cameras.GetArrayElementAtIndex(cameras.arraySize - 1).objectReferenceValue = camera;
        
        serializedObject.ApplyModifiedProperties();
        
        // Add room boundary
        GameObject transition = new GameObject();
        transition.name = $"Room Boundary [{cameras.arraySize - 2} - {cameras.arraySize - 1}]";
        transition.transform.parent = levelHeader;
        transition.AddComponent<BoxCollider2D>().isTrigger = true;
        RoomBoundary roomBound = transition.AddComponent<RoomBoundary>();
        SerializedObject roomBoundary = new SerializedObject(roomBound);
        roomBoundary.FindProperty("leftCameraIndex").intValue = cameras.arraySize - 2;
        roomBoundary.FindProperty("rightCameraIndex").intValue = cameras.arraySize - 1;
        roomBoundary.ApplyModifiedProperties();
        
        // Add checkpoint
        GameObject checkpoint = new GameObject();
        checkpoint.name = $"Checkpoint {cameras.arraySize - 1}";
        checkpoint.transform.parent = levelHeader;
        checkpoint.AddComponent<BoxCollider2D>().isTrigger = true;
        checkpoint.AddComponent<Checkpoint>();
    }
}
