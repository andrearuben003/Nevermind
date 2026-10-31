using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nevermind.Lights
{
    using Gravity;
    
    /// <summary>
    /// Experimental class for gravity interactions
    /// </summary>
    public class LightEmitterGravity : LightEmitter
    {
        private bool _justEnteredGravity = false;
        private readonly List<Gravity> _gravityInfluences = new();
        [SerializeField] private float gravityDistance = 0.1f;
        [SerializeField] private float gravityInfluenceStrength = 0.02f;
        private Vector2 _castDirection = Vector2.zero;
        private float _castDistance = 0f;
        
        public override void Awake()
        {
            base.Awake();

            BeamColor = Color.blue;
        }
        
        protected override void HandleBeamSpecificHitEvent(Collider2D currentCollider)
        {
            if (IsLightEnabled &&
                currentCollider.TryGetComponent(out IGravityLightTrigger trigger))
                trigger.OnGravityLightBeamHit();
        }
        
        protected override int SingleBeamCast(Vector2 origin, Vector2 direction)
        {
            // Reset
            _justEnteredGravity = false;
            _castDirection = direction;
            _castDistance = maxDistance;
            
            // Check gravity fields
            foreach (Gravity gravityInfluence in _gravityInfluences.ToList())
                if (!gravityInfluence.IsPointInGravityFields(origin))
                    _gravityInfluences.Remove(gravityInfluence);
            if (_gravityInfluences.Count > 0)
            {
                // Weighted average of the influences
                Vector2 force = _gravityInfluences
                                    .Select(gravity => (
                                        dir: gravity.GetDirectionTowardsCenter(origin), 
                                        force: gravity.attractionForce))
                                    .Aggregate(Vector2.zero, (a, b) => 
                                        a + b.dir*b.force)
                                / _gravityInfluences.Count;
                
                _castDirection = (direction + force*gravityInfluenceStrength).normalized;
                _castDistance = gravityDistance;
            }

            // Try to find a valid hit
            ContactFilter2D filter = ContactFilter2D.noFilter;
            filter.useLayerMask = true;
            filter.layerMask = ~0; // all layers
            int hitCount = Physics2D.Raycast(origin, _castDirection, filter, RaycastHits, _castDistance);
            for (int i = 0; i < hitCount; i++)
            {
                Collider2D currentCollider = RaycastHits[i].collider;
                
                // skip self
                if (currentCollider == BeamCollider) continue;
                
                // Event handling for raycast hits
                HandleBeamSpecificHitEvent(currentCollider);

                // Gravity check
                if (currentCollider.CompareTag("Gravity"))
                {
                    if (!currentCollider.TryGetComponent(out Gravity gravityInfluence)) 
                        throw new Exception("No Gravity class in gravity object");
                    if (_gravityInfluences.Contains(gravityInfluence)) continue;
                    _gravityInfluences.Add(gravityInfluence);
                    _justEnteredGravity = true;
                    return i;
                }
                

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
            Vector2 currentDirection = transform.right; // default for angle

            BeamPoints.Add(currentOrigin);

            for (int i = 0; i < maxBeamPoints; i++)
            {
                int hitIndex = SingleBeamCast(currentOrigin, currentDirection);
                
                if (hitIndex < 0)
                {
                    // In's normal not to hit anything in gravity
                    BeamPoints.Add(currentOrigin + _castDirection * _castDistance);
                    if (_gravityInfluences.Count == 0) break;
                    currentOrigin += _castDirection * gravityDistance;
                    currentDirection = _castDirection;
                    continue;
                }

                RaycastHit2D hit = RaycastHits[hitIndex];
                BeamPoints.Add(hit.point);
                
                if (_justEnteredGravity) {
                    // don't stop after hitting gravity
                    currentOrigin = hit.point + _castDirection * hitCollisionEpsilon;
                    currentDirection = _castDirection;
                    continue;
                }

                if (!hit.collider.CompareTag("Mirror")) break;

                currentOrigin = hit.point + hit.normal * hitCollisionEpsilon;
                currentDirection = Vector2.Reflect(currentDirection, hit.normal);
            }
        }
    }
}