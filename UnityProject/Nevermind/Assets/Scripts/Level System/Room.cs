using Unity.Cinemachine;
using UnityEngine;

namespace Nevermind.LevelSystem
{
    public class Room : MonoBehaviour
    {
        [SerializeField]
        private CinemachineCamera roomCamera;
        [SerializeField, Tooltip("If true, the controlled Bodies will contain all of the rigidbodies in the room's children.")]
        private bool autoCollectBodies = true;
        [SerializeField, Tooltip("Rigidbodies that will be set to dynamic/kinematic when the player enters/exits the room.")]
        private Rigidbody2D[] controlledBodies;
        [SerializeField, Tooltip("Components that will be enabled/disabled when player enters/exits the room.")]
        private Behaviour[] controlledComponents;

        private void Awake()
        {
            if (autoCollectBodies)
                controlledBodies = transform.GetComponentsInChildren<Rigidbody2D>(true);
        }

        /// Enables the room's camera and its controlled components and sets its controlled bodies to dynamic.
        public void EnableRoom()
        {
            roomCamera.enabled = true;
            
            foreach (Rigidbody2D body in controlledBodies)
                if (body != null)
                    body.bodyType = RigidbodyType2D.Dynamic;
            
            foreach (Behaviour component in controlledComponents)
                component.enabled = true;
        }

        /// Disables the room's camera and its controlled components and sets its controlled bodies to kinematic.
        public void DisableRoom()
        {
            roomCamera.enabled = false;
            
            foreach (Rigidbody2D body in controlledBodies)
            {
                if (body == null) continue;
                body.bodyType = RigidbodyType2D.Kinematic;
                body.linearVelocity = Vector2.zero;
                body.angularVelocity = 0f;
            }
            
            foreach (Behaviour component in controlledComponents)
                component.enabled = false;
        }
    }
}