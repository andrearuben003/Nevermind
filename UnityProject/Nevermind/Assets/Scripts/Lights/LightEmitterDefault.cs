using System;
using UnityEngine;

namespace Nevermind.Lights
{
    using Gravity;
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

        protected override void BeamCastHandleSpecificHitEvent(Collider2D currentCollider)
        {
            if (IsLightEnabled &&
                currentCollider.TryGetComponent(out IDefaultLightTrigger trigger))
                trigger.OnDefaultLightBeamHit();
        }

        protected override bool SingleBeamCastForceCollider(Collider2D currentCollider)
        {
            // Toxic spread
            if (!currentCollider.TryGetComponent<LightEmitterToxic>(out _)) 
                return false;
            
            _hasThisBeamHitToxic = true;
            return true;
        }

        public override void BeamCast()
        {
            _hasThisBeamHitToxic = false;
            toxicEmitter.FlushBeam();
            
            BeamPoints.Clear();

            Vector2 currentOrigin = transform.position;
            Vector2 currentDirection = transform.right; // default for angle
            float castDistance = 0f;

            BeamPoints.Add(currentOrigin);

            for (int i = 0; i < maxBeamPoints; i++)
            {
                // Gravity handling
                JustEnteredGravity = false;
                castDistance = maxDistance;
                for (int j = 0; j < GravityInfluences.Count; j++)
                    if (!GravityInfluences[j].IsPointInGravityFields(currentOrigin))
                        GravityInfluences.RemoveAt(j);
                if (GravityInfluences.Count > 0)
                {
                    // Weighted average of the influences
                    Vector2 force = Vector2.zero;
                    foreach (Gravity gravityInfluence in GravityInfluences)
                        force += gravityInfluence.GetDirectionTowardsCenter(currentOrigin) *
                                 gravityInfluence.attractionForce;
                    force /= GravityInfluences.Count;
                
                    currentDirection = (currentDirection + force*gravityInfluenceStrength).normalized;
                    castDistance = gravityStepDistance;
                }
                
                int hitIndex = SingleBeamCast(currentOrigin, currentDirection, castDistance);
                
                if (hitIndex < 0)
                {
                    // In's normal not to hit anything in gravity
                    BeamPoints.Add(currentOrigin + currentDirection * castDistance);
                    if (GravityInfluences.Count == 0) break;
                    currentOrigin += currentDirection * gravityStepDistance;
                    continue;
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
                
                if (JustEnteredGravity) {
                    // don't stop after hitting gravity
                    currentOrigin = hit.point + currentDirection * hitCollisionEpsilon;
                    continue;
                }

                if (!hit.collider.CompareTag("Mirror")) break;

                currentOrigin = hit.point + hit.normal * hitCollisionEpsilon;
                currentDirection = Vector2.Reflect(currentDirection, hit.normal);
            }
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