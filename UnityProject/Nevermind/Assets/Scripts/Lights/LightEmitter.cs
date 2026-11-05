using System;
using System.Collections.Generic;
using Nevermind.MemorySystem;
using UnityEngine;

namespace Nevermind.Lights
{
    using Gravity;

    [SelectionBase]
    [RequireComponent(typeof(EdgeCollider2D), typeof(LineRenderer))]
    public abstract class LightEmitter : MemoryListener
    {
        
        // Beam data
        protected EdgeCollider2D BeamCollider;
        protected LineRenderer BeamRenderer;
        protected Color BeamColor = Color.white;
        protected readonly List<Vector2> BeamPoints = new();
        protected readonly List<RaycastHit2D> RaycastHits = new();
        // Gravity related data
        protected bool JustEnteredGravity = false;
        protected readonly List<Gravity> GravityInfluences = new();

        // Memory bind
        protected bool IsLightEnabled = false;

        // Params and consts
        [SerializeField, Min(0)] protected int maxBeamPoints = 200;
        [SerializeField, Min(0)] protected float maxDistance = 100f;
        [SerializeField, Min(0)] private float widthMultiplier = 0.3f;
        [SerializeField, Min(0)] private float colliderEdgeRadius = 0.15f;
        [SerializeField, Min(0)] protected float hitCollisionEpsilon = 0.01f;
        [SerializeField, Range(0, 1)] protected float alphaOnDeactivation = 0.2f;
        [SerializeField, Min(0)] protected float gravityStepDistance = 0.8f;
        [SerializeField, Min(0)] protected float gravityInfluenceStrength = 0.16f;

        public virtual void Awake()
        {
            BeamCollider = GetComponent<EdgeCollider2D>();
            BeamRenderer = GetComponent<LineRenderer>();

            BeamCollider.isTrigger = true;
            
            BeamRenderer.useWorldSpace = true;
        }
        
        public virtual void FixedUpdate()
        {
            BeamCast();
            UpdateBeamCollider();
        }

        public virtual void Update()
        {
            UpdateBeamRenderer();
        }
        
        /** Beam-specific events with the given collider */
        protected abstract void BeamCastHandleSpecificHitEvent(Collider2D currentCollider);

        /** Signals whether this collider has to be manually ignored */
        protected virtual bool SingleBeamCastIgnoreCollider(Collider2D currentCollider) { return false; }

        /** Signals whether this collider has to be manually returned */
        protected virtual bool SingleBeamCastForceCollider(Collider2D currentCollider) { return false; }

        /**
         * Handles the raycast logic.
         * <returns> the first valid hit index; else -1 </returns>
         */
        protected virtual int SingleBeamCast(Vector2 origin, Vector2 direction, float castDistance)
        {
            // Try to find a valid hit
            ContactFilter2D filter = ContactFilter2D.noFilter;
            filter.useLayerMask = true;
            filter.layerMask = ~0; // all layers
            int hitCount = Physics2D.Raycast(origin, direction, filter, RaycastHits, castDistance);
            for (int i = 0; i < hitCount; i++)
            {
                Collider2D currentCollider = RaycastHits[i].collider;
                
                // skip self
                if (currentCollider == BeamCollider) continue;
                
                // Event handling for raycast hits
                BeamCastHandleSpecificHitEvent(currentCollider);

                // Custom collider handling
                if (SingleBeamCastIgnoreCollider(currentCollider)) continue;
                if (SingleBeamCastForceCollider(currentCollider)) return i;

                // Gravity check
                if (currentCollider.CompareTag("Gravity"))
                {
                    if (!currentCollider.TryGetComponent(out Gravity gravityInfluence)) 
                        throw new Exception("No Gravity class in gravity object");
                    if (GravityInfluences.Contains(gravityInfluence)) continue;
                    GravityInfluences.Add(gravityInfluence);
                    JustEnteredGravity = true;
                    return i;
                }
                
                // Skip triggers
                if (currentCollider.isTrigger) continue;

                return i;
            }

            return -1;
        }
        
        /** Casts the beam */
        public virtual void BeamCast()
        {
            BeamPoints.Clear();

            Vector2 currentOrigin = transform.position;
            Vector2 currentDirection = transform.right; // default for angle
            float castDistance = 0f;

            BeamPoints.Add(currentOrigin);

            for (int i = 0; i < maxBeamPoints; i++)
            {
                // Gravity setup
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
                
                if (JustEnteredGravity) {
                    // don't stop after hitting gravity
                    currentOrigin = hit.point + currentDirection * hitCollisionEpsilon;
                    continue;
                }

                if (!hit.collider.CompareTag("Mirror")) break;

                currentOrigin = hit.point + hit.normal * hitCollisionEpsilon;
                currentDirection = Vector2.Reflect(currentDirection, hit.normal);
            }
        }
        
        /** Updates the beam collider with the current points */
        public virtual void UpdateBeamCollider()
        {
            BeamCollider.enabled = IsLightEnabled;
            if (!IsLightEnabled) return;
            
            BeamCollider.edgeRadius = colliderEdgeRadius;
            
            // Add points
            List<Vector2> tempPoints = new();
            foreach (var point in BeamPoints)
                tempPoints.Add(transform.InverseTransformPoint(point)); // convert coordinates

            BeamCollider.SetPoints(tempPoints);
            tempPoints.Clear();
        }

        /** Updates the beam renderer with the current points */
        public virtual void UpdateBeamRenderer()
        {
            BeamRenderer.widthMultiplier = widthMultiplier;
            
            // Color
            BeamColor.a = IsLightEnabled ? 1 : alphaOnDeactivation;
            BeamRenderer.startColor = BeamRenderer.endColor = BeamColor;

            // Add points
            BeamRenderer.positionCount = BeamPoints.Count;
            for (int i = 0; i < BeamPoints.Count; i++)
                BeamRenderer.SetPosition(i, BeamPoints[i]);
        }

        protected override void onRemembered(MemoryKey key)
        {
            IsLightEnabled = true;
        }

        protected override void onForgot(MemoryKey key)
        {
            IsLightEnabled = false;
        }
    }
}