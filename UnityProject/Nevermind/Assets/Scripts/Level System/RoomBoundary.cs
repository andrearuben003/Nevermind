using UnityEngine;

namespace Nevermind.LevelSystem
{
    [SelectionBase, RequireComponent(typeof(Collider2D))]
    public class RoomBoundary : MonoBehaviour
    {
        [SerializeField, Tooltip("The room to the left of the boundary.")]
        private GameObject leftRoom;
        [SerializeField, Tooltip("The room to the right of the boundary.")]
        private GameObject rightRoom;
        
        private int leftCameraIndex;
        private int rightCameraIndex;

        private TagHandle playerTag;

        private void Awake()
        {
            playerTag = TagHandle.GetExistingTag("Player");
        }

        private void Start()
        {
            leftCameraIndex = RoomManager.Instance.GetRoomIndex(leftRoom);
            rightCameraIndex = RoomManager.Instance.GetRoomIndex(rightRoom);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;
            
            // Player enters from the right -> switch to left room
            // Player enters from the left  -> switch to right room
            float rel = Vector3.Dot(transform.right, other.transform.position - transform.position);
            RoomManager.Instance.SwitchRoom(rel > 0 ? leftCameraIndex : rightCameraIndex);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;
            
            // Player exits from the right -> switch to right room
            // Player exits from the left  -> switch to left room
            float rel = Vector3.Dot(transform.right, other.transform.position - transform.position);
            RoomManager.Instance.SwitchRoom(rel < 0 ? leftCameraIndex : rightCameraIndex);
        }
    }
}
