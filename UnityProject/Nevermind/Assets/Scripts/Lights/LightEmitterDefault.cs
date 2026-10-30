using System;
using UnityEngine;

namespace Nevermind.Lights
{
    /// <summary>
    /// Default beam. Can be intoxicated by hitting a toxic beam.
    /// </summary>
    public class LightEmitterDefault : LightEmitter
    {

        // Toxic child related attributes
        private bool _hasThisBeamHitToxic = false;
        [SerializeField] private LightEmitterDefaultToxic toxicEmitter = null;


        public override void Awake()
        {
            base.Awake();
            
            BeamColor = Color.green;

            if (!toxicEmitter) throw new Exception("No toxic emitter associated with this");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            toxicEmitter.UpdateBeamCollider();
        }

        public override void Update()
        {
            base.Update();
            toxicEmitter.UpdateBeamRenderer();
        }

        protected override void HandleBeamSpecificHitEvent(Collider2D currentCollider)
        {
            if (IsLightEnabled &&
                currentCollider.TryGetComponent<IDefaultLightTrigger>(out IDefaultLightTrigger trigger))
                trigger.OnDefaultLightBeamHit();
        }

        protected override int SingleBeamCast(Vector2 origin, Vector2 direction)
        {
            ContactFilter2D filter = ContactFilter2D.noFilter;
            filter.useLayerMask = true;
            filter.layerMask = ~0; // all layers

            int hitCount = Physics2D.Raycast(origin, direction, filter, RaycastHits, maxDistance);

            // Try to find a valid hit
            for (int i = 0; i < hitCount; i++)
            {
                Collider2D currentCollider = RaycastHits[i].collider;

                // Trigger
                HandleBeamSpecificHitEvent(currentCollider);

                // Toxic spread
                if (currentCollider.TryGetComponent<LightEmitterToxic>(out _))
                {
                    _hasThisBeamHitToxic = true;
                    return i;
                }

                // skip itself
                if (currentCollider == BeamCollider) continue;

                // Skip triggers
                if (currentCollider.isTrigger) continue;

                return i;
            }

            return -1;
        }

        public override void BeamCast()
        {
            _hasThisBeamHitToxic = false;
            toxicEmitter.FlushBeam();

            BeamPoints.Clear();

            Vector2 currentOrigin = transform.position;
            Vector2 currentDirection = transform.right;

            BeamPoints.Add(currentOrigin);

            for (int i = 0; i < maxBeamPoints; i++)
            {
                int hitIndex = SingleBeamCast(currentOrigin, currentDirection);
                if (hitIndex < 0) // not hit
                {
                    BeamPoints.Add(currentOrigin + currentDirection * maxDistance);
                    break;
                }

                RaycastHit2D hit = RaycastHits[hitIndex];
                BeamPoints.Add(hit.point);

                // Toxic check and cast
                if (_hasThisBeamHitToxic)
                {
                    toxicEmitter.transform.position = hit.point;
                    toxicEmitter.SetBeamDirection(currentDirection);
                    toxicEmitter.BeamCast();
                    break;
                }

                if (!hit.collider.CompareTag("Mirror")) break;

                // move it slightly (Epsilon) so that the ray doesn't hit the same thing again
                currentOrigin = hit.point + hit.normal * hitCollisionEpsilon;
                currentDirection = Vector2.Reflect(currentDirection, hit.normal);
            }
            
            // Toxic emitter is disabled if the default does not hit a toxic beam
            toxicEmitter.SetLightEnabled(IsLightEnabled && _hasThisBeamHitToxic);
        }

        /** Updates the beam collider with the current points */
        public override void UpdateBeamCollider()
        {
            base.UpdateBeamCollider();
            toxicEmitter.UpdateBeamCollider();
        }

        /** Updates the beam renderer with the current points */
        public override void UpdateBeamRenderer()
        {
            base.UpdateBeamRenderer();
            toxicEmitter.UpdateBeamRenderer();
        }
    }
}