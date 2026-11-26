using UnityEngine;

namespace Nevermind.Gravity
{
    public class CircularGravityField : GravityField
    {
        protected override Vector2 getGravityDirection(Vector2 point)
        {
            return ((Vector2)transform.position - point).normalized;
        }

        protected override void OnTriggerStay2D(Collider2D other)
        {
            base.OnTriggerStay2D(other);
            if (other.gameObject.TryGetComponent(out CharacterMovement movement))
                movement.ExternalInfluences[GetInstanceID()] = GetGravityForce(other.transform.position).x;
        }
    }
}
