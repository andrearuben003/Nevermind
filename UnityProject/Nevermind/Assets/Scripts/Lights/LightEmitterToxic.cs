using UnityEngine;

namespace Nevermind.Lights
{
    /// <summary>
    /// Toxic beam.
    /// </summary>
    public class LightEmitterToxic : LightEmitter
    {

        public override void Awake()
        {
            base.Awake();

            BeamColor = Color.red;
        }

        protected override void BeamCastHandleSpecificHitEvent(Collider2D currentCollider)
        {
            if (IsLightEnabled &&
                currentCollider.TryGetComponent(out IToxicLightTrigger trigger))
                trigger.OnToxicLightBeamHit();
        }
    }
}