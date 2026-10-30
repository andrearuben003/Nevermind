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

        protected override void HandleBeamSpecificHitEvent(Collider2D currentCollider)
        {
            if (IsLightEnabled &&
             currentCollider.TryGetComponent<ISolidLightTrigger>(out ISolidLightTrigger trigger))
                trigger.OnSolidLightBeamHit();
        }

    }
}