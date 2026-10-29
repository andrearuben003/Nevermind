using System.Collections.Generic;
using UnityEngine;

namespace Nevermind.Lights
{
    /// <summary>
    /// This class is used by the DefaultBeamEmitter as its toxic part.
    /// This emitter does nothing on its own and should not be used if not in pair with a DefaultBeamEmitter.
    /// </summary>
    public class LightEmitterDefaultToxic : LightEmitter
    {
        /** These beams **cannot** reinfect (I am NOT trying to make contamination chains (again)) */
        
        public EdgeCollider2D beamCollider = null;
        public Transform beamTransform = null;
        
        public override void Awake()
        {
            base.Awake();

            BeamColor = Color.orangeRed;
            
            beamTransform = transform;
            beamCollider = GetComponent<EdgeCollider2D>();
            
            // It's invisible when inactive
            alphaOnDeactivation = 0f;
        }

        public override void FixedUpdate()
        {
            // do nothing
            return;
        }

        public override void Update()
        {
            // do nothing
            return;
        }

        protected override void HandleBeamSpecificHitEvent(Collider2D currentCollider)
        {
            if (IsLightEnabled &&
                currentCollider.TryGetComponent<IToxicLightTrigger>(out IToxicLightTrigger trigger))
                trigger.OnToxicLightBeamHit();
        }

        /** Sets the beam's LightEnabled */
        public void SetLightEnabled(bool lightEnabled)
        {
            IsLightEnabled = lightEnabled;
        }

        public void SetBeamDirection(Vector2 direction)
        {
            transform.right = direction;
        }

        /** Forcefully flushes the beam */
        public void FlushBeam()
        {
            BeamPoints.Clear();
        }
    }
}