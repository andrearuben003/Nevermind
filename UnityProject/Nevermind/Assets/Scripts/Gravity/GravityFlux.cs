using UnityEngine;
using TarodevController;

namespace Nevermind.Gravity
{
    [SelectionBase]
    public class GravityFlux : MonoBehaviour
    {
        [SerializeField, Min(0)]
        private float attractionForce;
        
        private TagHandle playerTag;

        private void Awake()
        {
            playerTag = TagHandle.GetExistingTag("Player");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;
            
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.AddVelocity((Vector2)transform.up * attractionForce);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;
            
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.AddVelocity(-(Vector2)transform.up * attractionForce);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag(playerTag)) return; //Player handled separately
            
            Vector2 vel = (Vector2)transform.up * attractionForce;
            if (other.TryGetComponent(out Rigidbody2D rigidBody))
                rigidBody.AddForce(vel, ForceMode2D.Impulse);
        }
    }
}
