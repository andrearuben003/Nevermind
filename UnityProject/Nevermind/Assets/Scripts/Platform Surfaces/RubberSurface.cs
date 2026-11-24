using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nevermind.PlatformSurfaces
{
    public class RubberSurface : MonoBehaviour
    {
        [SerializeField, Min(0)] 
        private float bounceSpeed = 10;

        [SerializeField, Min(0)]
        private float playerMultiplier = 2;

        private Rigidbody2D rb;
        private TagHandle playerTag;
        
        void Awake()
        {
            rb = gameObject.GetComponentInParent<Rigidbody2D>();
            playerTag = TagHandle.GetExistingTag("Player");
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            Collider2D col = other.collider;
            if (col.attachedRigidbody == rb || !col.attachedRigidbody)
                return;
        
            List<ContactPoint2D> contacts = new();
            other.GetContacts(contacts);
            Vector2 normalAvg = contacts.Aggregate(Vector2.zero, (current, contact) => current + contact.normal) / contacts.Count;
            Vector2 bounce = -normalAvg * bounceSpeed;

            if(rb.bodyType == RigidbodyType2D.Dynamic)
                rb.AddForce(-bounce, ForceMode2D.Impulse);
        
            if (col.gameObject.CompareTag(playerTag))
                bounce *= playerMultiplier;
            
            if(col.attachedRigidbody.bodyType == RigidbodyType2D.Dynamic)
                col.attachedRigidbody.AddForce(bounce, ForceMode2D.Impulse);
        }
    }
}