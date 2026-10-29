using UnityEngine;

namespace Nevermind.LevelSystem
{
    [SelectionBase, RequireComponent(typeof(Collider2D))]
    public class RoomBoundary : MonoBehaviour
    {
        [SerializeField, Min(0), Tooltip("The index of the room to the left of the boundary.")]
        private int leftCameraIndex;
        [SerializeField, Min(0), Tooltip("The index of the room to the right of the boundary.")]
        private int rightCameraIndex;

        private TagHandle playerTag;

        private void Awake()
        {
            playerTag = TagHandle.GetExistingTag("Player");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;
            
            // Player enters from the right -> switch to left room
            // Player enters from the left  -> switch to right room
            float rel = Vector3.Dot(transform.right, other.transform.position - transform.position);
            CameraManager.Instance.SwitchCamera(rel > 0 ? leftCameraIndex : rightCameraIndex);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;
            
            // Player exits from the right -> switch to right room
            // Player exits from the left  -> switch to left room
            float rel = Vector3.Dot(transform.right, other.transform.position - transform.position);
            CameraManager.Instance.SwitchCamera(rel < 0 ? leftCameraIndex : rightCameraIndex);
        }
    }
}
