using UnityEngine;

namespace Nevermind.Gravity
 {
     public class LinearGravityField : GravityField
     {
         protected override Vector2 getGravityDirection(Vector2 point)
         {
             return transform.up;
         }
     }
 }