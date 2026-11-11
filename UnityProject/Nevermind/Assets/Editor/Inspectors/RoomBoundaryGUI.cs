using Nevermind.LevelSystem;
using UnityEditor;
using UnityEngine;

public class RoomBoundaryGUI
{
    [DrawGizmo(GizmoType.Active | GizmoType.Selected)]
    private static void OnDrawGizmosSelected(RoomBoundary boundary, GizmoType gizmoType)
    {
        Vector3 pos = boundary.transform.position;
        
        SerializedObject serializedObject = new SerializedObject(boundary);
        GameObject lCam = ((Component)serializedObject.FindProperty("leftCamera").objectReferenceValue).gameObject;
        GameObject rCam = ((Component)serializedObject.FindProperty("rightCamera").objectReferenceValue).gameObject;

        if (lCam)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pos, lCam.transform.position);
        }

        if (rCam)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, rCam.transform.position);
        }
    }
}
