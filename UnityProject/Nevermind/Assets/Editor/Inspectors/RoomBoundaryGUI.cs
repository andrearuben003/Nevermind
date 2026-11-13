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
        GameObject lCam = ((GameObject)serializedObject.FindProperty("leftRoom").objectReferenceValue).GetComponentInChildren<CinemachineCamera>().gameObject;
        GameObject rCam = ((GameObject)serializedObject.FindProperty("rightRoom").objectReferenceValue).GetComponentInChildren<CinemachineCamera>().gameObject;

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
