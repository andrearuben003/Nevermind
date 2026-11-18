using Nevermind.Player;
using UnityEditor;
using UnityEngine;

public static class PlayerInteractionGUI
{
    [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    private static void drawGizmo(PlayerInteraction interaction, GizmoType gizmoType)
    {
        SerializedObject serializedObject = new(interaction);
        float radius = serializedObject.FindProperty("radius").floatValue;
        Vector2 offset = serializedObject.FindProperty("offset").vector2Value;
        
        Gizmos.color = Color.yellow;
        
        Vector3 origin = interaction.transform.position + new Vector3(offset.x, offset.y, 0);
        Gizmos.DrawWireSphere(origin, radius);
    }
}
