using System.Collections.Generic;
using Nevermind.MemorySystem;
using UnityEngine;

namespace Nevermind.Lights
{

    [SelectionBase]
    [RequireComponent(typeof(EdgeCollider2D), typeof(LineRenderer))]
    public abstract class LightEmitter : MemoryListener
    {
        // Beam data
        protected EdgeCollider2D BeamCollider;
        protected LineRenderer BeamRenderer;
        protected List<Vector2> BeamPoints;
        protected Color BeamColor = Color.white;
        protected readonly List<RaycastHit2D> RaycastHits = new();

        // Memory bind
        protected bool IsLightEnabled = false;

        // Params and consts
        [SerializeField, Min(0)] protected float maxDistance = 100f;
        [SerializeField, Min(0)] protected int maxBeamPoints = 200;
        [SerializeField, Min(0)] protected float hitCollisionEpsilon = 0.01f;
        [SerializeField, Min(0)] private float widthMultiplier = 0.3f;
        [SerializeField, Min(0)] private float colliderEdgeRadius = 0.15f;
        [SerializeField, Range(0, 1)] protected float alphaOnDeactivation = 0.2f;

        public virtual void Awake()
        {
            BeamCollider = GetComponent<EdgeCollider2D>();
            BeamRenderer = GetComponent<LineRenderer>();
            BeamPoints = new List<Vector2>();

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
        
        /** Beam-specific events */
        protected abstract void HandleBeamSpecificHitEvent(Collider2D currentCollider);

        /**
         * Handles the raycast logic.
         * <returns> the first valid hit index; else -1 </returns>
         */
        protected virtual int SingleBeamCast(Vector2 origin, Vector2 direction)
        {
            ContactFilter2D filter = ContactFilter2D.noFilter;
            filter.useLayerMask = true;
            filter.layerMask = ~0; // all layers

            int hitCount = Physics2D.Raycast(origin, direction, filter, RaycastHits, maxDistance);
            // Try to find a valid hit
            for (int i = 0; i < hitCount; i++)
            {
                Collider2D currentCollider = RaycastHits[i].collider;
                
                // Event handling for raycast hits
                HandleBeamSpecificHitEvent(currentCollider);
                
                // skip self
                if (currentCollider == BeamCollider) continue;

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

                if (!hit.collider.CompareTag("Mirror")) break;

                // move it slightly (Epsilon) so that the ray doesn't hit the same thing again
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