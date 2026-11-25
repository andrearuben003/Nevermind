using Nevermind.LevelSystem;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

public static class RoomBoundaryGUI
{
    [DrawGizmo(GizmoType.Active | GizmoType.Selected)]
    private static void OnDrawGizmosSelected(RoomBoundary boundary, GizmoType gizmoType)
    {
        Vector3 pos = boundary.transform.position;
        
        SerializedObject serializedObject = new(boundary);
        Object lO = serializedObject.FindProperty("leftRoom").objectReferenceValue;
        Object rO = serializedObject.FindProperty("rightRoom").objectReferenceValue;
        GameObject lCam = lO ? ((Room)lO).GetComponentInChildren<CinemachineCamera>().gameObject : null;
        GameObject rCam = rO ? ((Room)rO).GetComponentInChildren<CinemachineCamera>().gameObject : null;

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
