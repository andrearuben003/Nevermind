using UnityEngine;

namespace Nevermind.Gravity
 {
     [SelectionBase]
     public class GravityFlux : Gravity
     {
         private const float UpDistance = 1000f; // far enough
         
         public override void FixedUpdate()
         {
             // Always point up
             Center = transform.position;
             Center += Vector2.up * UpDistance;
         }
     }
 }