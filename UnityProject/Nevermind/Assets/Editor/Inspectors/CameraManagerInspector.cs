using Nevermind.LevelSystem;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraManager))]
public class CameraManagerInspector : Editor
{
    [SerializeField]
    private Texture2D boundaryIcon;
    [SerializeField]
    private Texture2D checkpointIcon;
    
    private Transform levelHeader;
    
    public override void OnInspectorGUI()
    {
        if (!levelHeader)
            levelHeader = GameObject.Find("---- LEVEL MAP ----").transform;
        
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Add Room"))
            addRoom();
        
        SerializedProperty cameras = serializedObject.FindProperty("roomCameras");
        for (int i = 0; i < cameras.arraySize; i++)
        {
            if (cameras.GetArrayElementAtIndex(i).objectReferenceValue == null)
                cameras.DeleteArrayElementAtIndex(i);
        }
        serializedObject.ApplyModifiedProperties();
    }
    
    private void addRoom()
    {
        SerializedProperty cameras = serializedObject.FindProperty("roomCameras");
        
        GameObject room = new GameObject();
        room.name = $"---- ROOM {cameras.arraySize} ----";
        room.transform.parent = levelHeader;
        
        // Add new camera
        GameObject newCam = new GameObject();
        newCam.name = $"Camera {cameras.arraySize}";
        newCam.transform.parent = room.transform;
        newCam.transform.position = new Vector3(0, -3, -10);
        CinemachineCamera camera = newCam.AddComponent<CinemachineCamera>();
        camera.Lens.OrthographicSize = 5f;
        
        cameras.InsertArrayElementAtIndex(cameras.arraySize);
        cameras.GetArrayElementAtIndex(cameras.arraySize - 1).objectReferenceValue = camera;
        serializedObject.ApplyModifiedProperties();
        
        // Add room boundary
        GameObject transition = new GameObject();
        EditorGUIUtility.SetIconForObject(transition, boundaryIcon);
        transition.name = $"Room Boundary [{cameras.arraySize - 2} - {cameras.arraySize - 1}]";
        transition.transform.parent = room.transform;
        transition.transform.position = new Vector3(3, -3, 0);
        transition.AddComponent<BoxCollider2D>().isTrigger = true;
        RoomBoundary roomBound = transition.AddComponent<RoomBoundary>();
        SerializedObject roomBoundary = new SerializedObject(roomBound);
        roomBoundary.FindProperty("leftCamera").objectReferenceValue = cameras.GetArrayElementAtIndex(cameras.arraySize - 2).objectReferenceValue;
        roomBoundary.FindProperty("rightCamera").objectReferenceValue = cameras.GetArrayElementAtIndex(cameras.arraySize - 1).objectReferenceValue;
        roomBoundary.ApplyModifiedProperties();
        
        // Add checkpoint
        GameObject checkpoint = new GameObject();
        EditorGUIUtility.SetIconForObject(checkpoint, checkpointIcon);
        checkpoint.name = $"Checkpoint {cameras.arraySize - 1}";
        checkpoint.transform.parent = room.transform;
        checkpoint.transform.position = new Vector3(6, -3, 0);
        checkpoint.AddComponent<BoxCollider2D>().isTrigger = true;
        checkpoint.AddComponent<Checkpoint>();
    }
}
