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
        GameObject room = new()
        {
            name = $"---- ROOM {rooms.arraySize} ----",
            transform =
            {
                parent = levelHeader
            }
        };

        rooms.InsertArrayElementAtIndex(rooms.arraySize);
        rooms.GetArrayElementAtIndex(rooms.arraySize - 1).objectReferenceValue = room;
        serializedObject.ApplyModifiedProperties();
        
        // Add new camera
        GameObject newCam = new()
        {
            name = $"Camera {rooms.arraySize - 1}",
            transform =
            {
                parent = room.transform,
                position = new Vector3(0, -3, -10)
            }
        };
        CinemachineCamera camera = newCam.AddComponent<CinemachineCamera>();
        camera.Lens.OrthographicSize = 5f;
        
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
        transition.AddComponent<BoxCollider2D>().isTrigger = true;
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
                parent = room.transform,
                position = new Vector3(6, -3, 0)
            }
        };
        EditorGUIUtility.SetIconForObject(checkpoint, checkpointIcon);
        checkpoint.AddComponent<BoxCollider2D>().isTrigger = true;
        checkpoint.AddComponent<Checkpoint>();
    }
}
