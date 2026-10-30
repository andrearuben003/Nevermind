using UnityEngine;

namespace Nevermind.Lights
{
    /// <summary>
    /// This class is used by the DefaultBeamEmitter as its toxic part.
    /// This emitter does nothing on its own and should not be used if not in pair with a DefaultBeamEmitter.
    /// </summary>
    public class LightEmitterDefaultToxic : LightEmitter
    {
        public override void Awake()
        {
            base.Awake();

            BeamColor = Color.orangeRed;

            // It's invisible when inactive
            alphaOnDeactivation = 0f;
        }

        public override void FixedUpdate()
        {
            // do nothing
        }

        public override void Update()
        {
            // do nothing
        }

        protected override void HandleBeamSpecificHitEvent(Collider2D currentCollider)
        {
            if (IsLightEnabled &&
                currentCollider.TryGetComponent(out IToxicLightTrigger trigger))
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