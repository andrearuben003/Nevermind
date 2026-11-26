using System;
using TarodevController;
using UnityEngine;

namespace Nevermind.Gravity
{
    [SelectionBase, RequireComponent(typeof(Collider2D))]
    public abstract class GravityField : MonoBehaviour
    {
        [SerializeField, Min(0)] 
        private float attractionForce = 1.0f;

        private Collider2D gravityCollider;

        public virtual void Awake()
        {
            gravityCollider = GetComponent<Collider2D>();
        }

        protected abstract Vector2 getGravityDirection(Vector2 point);

        public bool IsPointInGravityField(Vector2 point) => gravityCollider.OverlapPoint(point);
        
        public Vector2 GetGravityForce(Vector2 point) => getGravityDirection(point) * attractionForce;
        
        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (!other.attachedRigidbody) return;
            
            Vector2 force = GetGravityForce(other.transform.position);
            other.attachedRigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.attachedRigidbody) return;

            other.attachedRigidbody.gravityScale = 0;

            if (other.gameObject.TryGetComponent(out CharacterJump jump))
                jump.LockGravity = true;
            if (other.gameObject.TryGetComponent(out CharacterMovement movement))
                movement.ExternalInfluences.Add(GetInstanceID(), GetGravityForce(other.transform.position).x);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.attachedRigidbody) return;
            
            other.attachedRigidbody.gravityScale = 1;
            
            if (other.gameObject.TryGetComponent(out CharacterJump jump))
                jump.LockGravity = false;
            if (other.gameObject.TryGetComponent(out CharacterMovement movement))
                movement.ExternalInfluences.Remove(GetInstanceID());
        }
    }
}
