using UnityEngine;

namespace Nevermind.Lights
{
    public class LightTrigger : MonoBehaviour
    {
        [SerializeField, Tooltip("Trigger sprite")]
        private SpriteRenderer triggerSprite;

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Light")) return;

            triggerSprite.color = Color.green;
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Light")) return;

            triggerSprite.color = Color.red;
        }

        // Colliders and triggers both activate the trigger
        public void OnCollisionEnter2D(Collision2D other)
        {
            OnTriggerEnter2D(other.collider);
        }

        public void OnCollisionExit2D(Collision2D other)
        {
            OnTriggerExit2D(other.collider);
        }
    }
}