using UnityEngine;

namespace Nevermind.Lights
{
    /// <summary>
    /// Toxic beam. It has collisions.
    /// </summary>
    public class LightEmitterSolid : LightEmitter
    {
        public override void Awake()
        {
            base.Awake();

            BeamColor = Color.yellow;
            
            // Solid beam
            BeamCollider.isTrigger = false;
        }

        protected override void BeamCastHandleSpecificHitEvent(Collider2D currentCollider)
        {
            if (IsLightEnabled &&
             currentCollider.TryGetComponent(out ISolidLightTrigger trigger))
                trigger.OnSolidLightBeamHit();
        }

        protected override bool SingleBeamCastIgnoreCollider(Collider2D currentCollider)
        {
            // Ignore other solid beams
            if (currentCollider.TryGetComponent<LightEmitterSolid>(out _))
                return true;
            return false;
        }
    }
}