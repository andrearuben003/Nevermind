using System.Collections.Generic;
using Nevermind.MemorySystem;
using UnityEngine;

namespace Nevermind.Lights
{
    public enum LightType
    {
        Default,
        Toxic,
        Solid
    }

    /// <summary>
    /// This class handles all the Light logic.
    /// For the Emitter to work, it must be in the LightEmitterManager's list.
    /// </summary>
    public class LightEmitter : MemoryListener
    {
        /*
         * Notes on the class:
         * - the solid light collider is weird because the player sometimes can go through; this should stop
         *     being an issue if we move to a normal physics based movement system
         * - I decided to develop the logic around the emitter instead of splitting it between emitter and beam
         *     as it's easier with no huge costs (I think)
         */

        // light color values
        private static readonly Dictionary<LightType, Color> LightColors = new()
        {
            { LightType.Default, Color.green },
            { LightType.Solid, Color.yellow },
            { LightType.Toxic, Color.red }
        };

        // consts
        private const float MaxDistance = 100f;
        private const int MaxBeamPoints = 20;
        private const float Epsilon = 0.01f;
        
        // Attributes
        [SerializeField] private LineRenderer beam;
        [SerializeField] private EdgeCollider2D beamCollider;

        // Note that changing this at runtime is undefined behaviour, it may or may not do something
        [SerializeField] public LightType originalLightType = LightType.Default;

        // light type (may change at runtime)
        public LightType LightType { get; private set; }
        public float alphaOnDeactivation = 0.2f;

        // how many toxic rays are touching this
        private int _toxicCount = 0;
        
        private bool _isLightEnabled = false;
        
        private readonly List<Vector2> _beamPoints = new();

        // Raycast attributes
        private ContactFilter2D _filter;
        private void Awake()
        {
            LightType = originalLightType;

            _filter = ContactFilter2D.noFilter;
            _filter.useLayerMask = true;
            _filter.layerMask = ~0;
            beam.useWorldSpace = true;
        }

        /**
         * Handles the raycast logic: it detects valid hits and handles toxic beam logic.
         * A valid hit is a hit that is not the beam itself and that is on a non-trigger collider.
         * <returns> the first valid hit index; else -1 </returns>
         */
        private int HandleBeamRaycast(int hitCount)
        {
            for (int i = 0; i < hitCount; i++)
            {
                Collider2D currentCollider = _raycastHits[i].collider;
                if (currentCollider == beamCollider)
                    continue;
                
                // check toxic light while iterating
                currentCollider.TryGetComponent<LightBeam>(out LightBeam otherBeam);
                if (otherBeam && otherBeam.emitter && 
                    otherBeam.emitter.LightType == LightType.Toxic)
                    _toxicCount++;
                
                if (!currentCollider.isTrigger)
                    return i;
            }

            return -1;
        }

        private readonly List<Vector2> _tempPoints = new();

        /**
         * Updates the beam collider
         */
        private void UpdateBeamCollider()
        {
            for (int i = 0; i < _beamPoints.Count; i++)
                _tempPoints.Add(transform.InverseTransformPoint(_beamPoints[i])); // convert coordinates
            beamCollider.SetPoints(_tempPoints);
            _tempPoints.Clear();
        }

        /**
         * Updates the beam rendering
         */
        private void UpdateBeam()
        {
            beam.positionCount = _beamPoints.Count;
            for (int i = 0; i < _beamPoints.Count; i++)
                beam.SetPosition(i, _beamPoints[i]);
        }

        /**
         * Handles the spread of the toxic status to default beams
         */
        private void ToxicSpread()
        {
            // If at least one toxic beam was touched, spread and reset count
            if (originalLightType != LightType.Default) return;
            LightType = _toxicCount > 0 ? LightType.Toxic : LightType.Default;
            _toxicCount = 0;
        }

        private readonly List<RaycastHit2D> _raycastHits = new();

        /**
         * Physics update
         */
        public void EmitterFixedUpdate()
        {
            if (!beam) return;

            if (LightType == LightType.Solid)
                beamCollider.isTrigger = false;
            else
                beamCollider.isTrigger = true;

            _beamPoints.Clear();

            // Check for light being enabled
            beamCollider.enabled = _isLightEnabled;
            
            // Cast beam:
            // I do this on the FixedUpdate because even when deactivated it's still visible

            // Init first cast
            Vector2 origin = transform.position;
            Vector2 direction = transform.right;

            _beamPoints.Add(origin);

            RaycastHit2D hit;
            for (int i = 0; i < MaxBeamPoints; i++) // avoid infinite loops
            {
                // get hits
                int hitCount = Physics2D.Raycast(origin, direction, _filter, _raycastHits, MaxDistance);
                int firstValidIndex = HandleBeamRaycast(hitCount);
                if (firstValidIndex < 0)
                {
                    _beamPoints.Add(origin + direction * MaxDistance);
                    break;
                }

                // Wall hit: add valid point
                hit = _raycastHits[firstValidIndex];
                _beamPoints.Add(hit.point);

                if (!hit.collider.CompareTag("Mirror")) break;

                origin = hit.point +
                         hit.normal * Epsilon; // move it slightly so that the ray doesn't hit the same thing again
                direction = Vector2.Reflect(direction, hit.normal);
            }

            ToxicSpread();
            UpdateBeamCollider();
        }

        /**
         * Rendering update
         */
        public void EmitterUpdate()
        {
            Color color = LightColors[LightType];
            color.a = _isLightEnabled ? 1 : alphaOnDeactivation;
            beam.startColor = beam.endColor = color;

            UpdateBeam();
        }

        protected override void onRemembered(MemoryKey key)
        {
            _isLightEnabled = true;
        }

        protected override void onForgot(MemoryKey key)
        {
            _isLightEnabled = false;
        }
    }
}