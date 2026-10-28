using System.Collections.Generic;
using UnityEngine;

namespace Nevermind.Lights
{
    public class LightEmitterSolid : LightEmitter
    {
        private readonly List<RaycastHit2D> _raycastHits = new();

        public void Awake()
        {
            base.Awake();

            BeamCollider.isTrigger = false;
            BeamColor = Color.yellow;
        }

        /**
         * Handles the raycast logic.
         * <returns> the first valid hit index; else -1 </returns>
         */
        private int SingleBeamCast(Vector2 origin, Vector2 direction)
        {
            ContactFilter2D filter = ContactFilter2D.noFilter;
            filter.useLayerMask = true;
            filter.layerMask = ~0; // all layers

            int hitCount = Physics2D.Raycast(origin, direction, filter, _raycastHits, maxDistance);
            // Try to find a valid hit
            for (int i = 0; i < hitCount; i++)
            {
                Collider2D currentCollider = _raycastHits[i].collider;
                
                // Trigger
                if (IsLightEnabled &&
                    currentCollider.TryGetComponent<ISolidLightTrigger>(out ISolidLightTrigger trigger))
                    trigger.OnSolidLightBeamHit(direction);
                
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
            BeamPoints.Clear();

            Vector2 currentOrigin = transform.position;
            Vector2 currentDirection = defaultFirstDirection;

            BeamPoints.Add(currentOrigin);

            for (int i = 0; i < maxBeamPoints; i++)
            {
                int hitIndex = SingleBeamCast(currentOrigin, currentDirection);
                if (hitIndex < 0) // not hit
                {
                    BeamPoints.Add(currentOrigin + currentDirection * maxDistance);
                    break;
                }

                RaycastHit2D hit = _raycastHits[hitIndex];
                BeamPoints.Add(hit.point);

                if (!hit.collider.CompareTag("Mirror")) break;

                // update
                // move it slightly (Epsilon) so that the ray doesn't hit the same thing again
                currentOrigin = hit.point + hit.normal * hitCollisionEpsilon;
                currentDirection = Vector2.Reflect(currentDirection, hit.normal);
            }
        }

        /** Updates the beam collider with the current points */
        public override void UpdateBeamCollider()
        {
            BeamCollider.enabled = IsLightEnabled;
            if (!IsLightEnabled) return;
            
            // Add points
            List<Vector2> tempPoints = new();
            foreach (var point in BeamPoints)
                tempPoints.Add(transform.InverseTransformPoint(point)); // convert coordinates

            BeamCollider.SetPoints(tempPoints);
            tempPoints.Clear();
        }

        /** Updates the beam renderer with the current points */
        public override void UpdateBeamRenderer()
        {
            // Color
            BeamColor.a = IsLightEnabled ? 1 : alphaOnDeactivation;
            BeamRenderer.startColor = BeamRenderer.endColor = BeamColor;

            // Add points
            BeamRenderer.positionCount = BeamPoints.Count;
            for (int i = 0; i < BeamPoints.Count; i++)
                BeamRenderer.SetPosition(i, BeamPoints[i]);
        }
    }
}
