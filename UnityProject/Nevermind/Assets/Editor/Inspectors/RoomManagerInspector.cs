using Nevermind.LevelSystem;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomManager))]
public class RoomManagerInspector : Editor
{
    [SerializeField]
    private Texture2D boundaryIcon;
    [SerializeField]
    private Texture2D checkpointIcon;
    
    private Transform levelHeader;
    private SerializedProperty rooms;
    
    public override void OnInspectorGUI()
    {
        if (!levelHeader)
            levelHeader = GameObject.Find("---- LEVEL MAP ----").transform;
        rooms = serializedObject.FindProperty("rooms");
            
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Add Room"))
            addRoom();
        
        for (int i = 0; i < rooms.arraySize; i++)
        {
            if (rooms.GetArrayElementAtIndex(i).objectReferenceValue == null)
                rooms.DeleteArrayElementAtIndex(i);
        }
        serializedObject.ApplyModifiedProperties();
    }
    
    private void addRoom()
    {
        GameObject roomObj = new()
        {
            name = $"---- ROOM {rooms.arraySize} ----",
            transform =
            {
                parent = levelHeader
            }
        };
        Room room = roomObj.AddComponent<Room>();
        SerializedObject roomSO = new(room);
        
        rooms.InsertArrayElementAtIndex(rooms.arraySize);
        rooms.GetArrayElementAtIndex(rooms.arraySize - 1).objectReferenceValue = room;
        serializedObject.ApplyModifiedProperties();
        
        // Add new camera
        GameObject newCam = new()
        {
            name = $"Camera {rooms.arraySize - 1}",
            transform =
            {
                parent = roomObj.transform,
                position = new Vector3(0, -3, -10)
            }
        };
        CinemachineCamera camera = newCam.AddComponent<CinemachineCamera>();
        camera.Lens.OrthographicSize = 5f;
        roomSO.FindProperty("roomCamera").objectReferenceValue = camera;
        roomSO.ApplyModifiedProperties();
        
        // Add room boundary
        GameObject transition = new()
        {
            name = $"Room Boundary [{rooms.arraySize - 2} - {rooms.arraySize - 1}]",
            transform =
            {
                parent = levelHeader,
                position = new Vector3(3, -3, 0)
            }
        };
        EditorGUIUtility.SetIconForObject(transition, boundaryIcon);
        transition.transform.SetSiblingIndex(transition.transform.GetSiblingIndex() - 1);
        BoxCollider2D coll = transition.AddComponent<BoxCollider2D>();
        coll.isTrigger = true;
        coll.size = new(1, 3);
        RoomBoundary roomBound = transition.AddComponent<RoomBoundary>();
        
        SerializedObject roomBoundary = new(roomBound);
        roomBoundary.FindProperty("leftRoom").objectReferenceValue = rooms.GetArrayElementAtIndex(rooms.arraySize - 2).objectReferenceValue;
        roomBoundary.FindProperty("rightRoom").objectReferenceValue = rooms.GetArrayElementAtIndex(rooms.arraySize - 1).objectReferenceValue;
        roomBoundary.ApplyModifiedProperties();
        
        // Add checkpoint
        GameObject checkpoint = new()
        {
            name = $"Checkpoint {rooms.arraySize - 1}",
            transform =
            {
                parent = roomObj.transform,
                position = new Vector3(6, -3, 0)
            }
        };
        EditorGUIUtility.SetIconForObject(checkpoint, checkpointIcon);
        coll = checkpoint.AddComponent<BoxCollider2D>();
        coll.isTrigger = true;
        coll.size = new(1, 3);
        coll.offset = new(0, 1.5f);
        checkpoint.AddComponent<Checkpoint>();
    }
}