using Nevermind.PlatformMovements;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformGUI: Editor
{
    private const int LineSize = 5;

    private void OnSceneGUI()
    {
        SerializedProperty path = serializedObject.FindProperty("pathTargets");
        for (int i = 0; i < path.arraySize; i++)
        {
            Vector3 point = path.GetArrayElementAtIndex(i).vector3Value;
            
            path.GetArrayElementAtIndex(i).vector3Value = Handles.DoPositionHandle(point, Quaternion.identity);
        }

        serializedObject.ApplyModifiedProperties();
    }
    
    [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    public static void DrawGizmos(MovingPlatform movingPlatform, GizmoType gizmoType)
    {
        SerializedObject serializedObject = new(movingPlatform);
        SerializedProperty path = serializedObject.FindProperty("pathTargets");
        SerializedProperty backtrack = serializedObject.FindProperty("backtrack");
        
        Gizmos.color = Color.white;
        Handles.color = Color.white;
        for (int i = 0; i < path.arraySize; i++)
        {
            Vector3 point = path.GetArrayElementAtIndex(i).vector3Value;
            
            if (i + 1 < path.arraySize)
            {
                Vector3 next = path.GetArrayElementAtIndex(i + 1).vector3Value;
                Handles.DrawDottedLine(point, next, LineSize);
            }
            
            Gizmos.DrawSphere(point, 0.1f);
            
            //targets.GetArrayElementAtIndex(i).vector3Value = Handles.DoPositionHandle(point, Quaternion.identity);
        }

        if (path.arraySize == 0) return;
        
        Vector3 first = path.GetArrayElementAtIndex(0).vector3Value;
        Handles.color = Color.red;
        Handles.DrawDottedLine(first, movingPlatform.transform.position, LineSize);

        if (backtrack.boolValue || path.arraySize == 1) return;
        
        Vector3 last = path.GetArrayElementAtIndex(path.arraySize - 1).vector3Value;
        Handles.color = Color.aquamarine;
        Handles.DrawDottedLine(first, last, LineSize);
    }
}
