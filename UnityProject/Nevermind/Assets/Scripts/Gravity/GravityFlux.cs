using UnityEngine;
using TarodevController;

namespace Nevermind.Gravity
 {
     [SelectionBase]
     public class GravityFlux : MonoBehaviour
     {
         [SerializeField, Min(0)]
         private float attractionForce;
 
         private void OnTriggerStay2D(Collider2D other)
         {
             Vector2 force = (Vector2)transform.up * attractionForce;
             if (other.TryGetComponent(out PlayerController controller))
                 controller.ApplyGravityFlow(force);
             else if (other.TryGetComponent(out Rigidbody2D rigidBody))
                 rigidBody.AddForce(force, ForceMode2D.Impulse);
         }
     }
 }