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

        protected override void HandleBeamSpecificHitEvent(Collider2D currentCollider)
        {
            if (IsLightEnabled &&
                currentCollider.TryGetComponent<IToxicLightTrigger>(out IToxicLightTrigger trigger))
                trigger.OnToxicLightBeamHit();
        }
    }
}