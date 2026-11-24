using Nevermind.Interactables;
using UnityEngine;

//This script is used by both movement and jump to detect when the character is touching the ground

public class CharacterGround : MonoBehaviour
{
        private bool onGround;
       
        [Header("Collider Settings")]
        [SerializeField][Tooltip("Length of the ground-checking collider")] public float groundLength = 0.95f;
        [SerializeField][Tooltip("Distance between the ground-checking colliders")] public Vector3 colliderOffset;

        [Header("Layer Masks")]
        [SerializeField][Tooltip("Which layers are read as the ground")] public LayerMask groundLayer;
 

        private void Update()
        {
            //Determine if the player is stood on objects on the ground layer, using a pair of raycasts
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

            onGround = hit1 || hit2;

            //This part of the code is needed to solve infinite box jump bug!!!
            //The player cannot jump on grabbed object
            if (hit1)
            {
                Grabbable tmp1 = hit1.transform.GetComponent<Grabbable>();

                if (tmp1 && tmp1.IsGrabbed)
                    onGround = false;
            } 
            else if (hit2) 
            {
                Grabbable tmp2 = hit2.transform.GetComponent<Grabbable>();

                if (tmp2 && tmp2.IsGrabbed)
                    onGround = false;
            }       
        }

        private void OnDrawGizmos()
        {
            //Draw the ground colliders on screen for debug purposes
            if (onGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
            Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
            Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
        }

        //Send ground detection to other scripts
        public bool GetOnGround() { return onGround; }
}