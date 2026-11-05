using TarodevController;
using UnityEngine;

namespace Nevermind.Gravity
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D))]
    public class Gravity : MonoBehaviour
    {
        [SerializeField, Min(0)] public float attractionForce = 1.0f;
        protected Vector2 Center =  Vector2.zero;

        private Collider2D _gravityCollider;

        public virtual void Awake()
        {
            _gravityCollider = GetComponent<Collider2D>();
        }

        public virtual void FixedUpdate()
        {
            Center = transform.position;
        }

        public Vector2 GetDirectionTowardsCenter(Vector2 other)
        {
            return ((Center) - other).normalized;
        }

        public bool IsPointInGravityFields(Vector2 point)
        {
            return _gravityCollider.OverlapPoint(point);
        }
        
        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            Vector2 force = GetDirectionTowardsCenter(other.transform.position) * attractionForce;
            if (other.TryGetComponent(out PlayerController controller))
                controller.ApplyGravityFlow(force);
            else if (other.TryGetComponent(out Rigidbody2D rigidBody))
                rigidBody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
