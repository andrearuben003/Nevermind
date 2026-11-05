using System;
using UnityEngine;

namespace Nevermind.Lights
{
    using Gravity;
    
    /// <summary>
    /// Experimental class for gravity interactions
    /// </summary>
    [Obsolete("Gravity logic has been moved to all the beams")]
    public class LightEmitterGravity : LightEmitter
    {
        
        public override void Awake()
        {
            base.Awake();

            BeamColor = Color.blue;
        }
        
        protected override void BeamCastHandleSpecificHitEvent(Collider2D currentCollider)
        {
            if (IsLightEnabled &&
                currentCollider.TryGetComponent(out IGravityLightTrigger trigger))
                trigger.OnGravityLightBeamHit();
        }
        
    }
}