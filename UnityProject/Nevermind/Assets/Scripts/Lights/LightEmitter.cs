using System.Collections.Generic;
using Nevermind.MemorySystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Nevermind.Lights
{
    public enum LightType
    {
        Default,
        Toxic,
        Solid
    }

    public class LightEmitter : MemoryListener
    {
        /**
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

        // Raycast attributes
        private ContactFilter2D _filter;

        // Attributes
        [SerializeField] private LineRenderer beam;

        [SerializeField] private EdgeCollider2D beamCollider;

        // Note that changing this at runtime is undefined behaviour, it may or may not do something
        [SerializeField] public LightType originalLightType = LightType.Default;

        // light type (may change at runtime)
        public LightType lightType;
        [SerializeField] public float alphaOnDeactivation = 0.2f;

        private readonly List<Vector2> _points = new();

        // how many toxic rays are touching this
        public int _toxicCount = 0;
        private bool _isLightEnabled = false;

        private void Awake()
        {
            lightType = originalLightType;

            _filter = ContactFilter2D.noFilter;
            _filter.useLayerMask = true;
            _filter.layerMask = ~0;
            beam.useWorldSpace = true;
        }

        /**
         * Returns the first hit that is not itself (or -1 if there is no valid hit)
         */
        private int FirstValidHit(int hitCount)
        {
            for (int i = 0; i < hitCount; i++)
            {
                if (_raycastHits[i].collider == beamCollider)
                    continue;
                // check toxic light
                if (_raycastHits[i].collider.GetComponent<LightBeam>()?.emitter.lightType == LightType.Toxic)
                    _toxicCount++;
                // check valid raycast
                if (_raycastHits[i].collider != beamCollider && !_raycastHits[i].collider.isTrigger)
                    return i;
            }

            return -1;
        }

        private readonly List<Vector2> _tempPoints = new();

        private void UpdateBeamCollider()
        {
            for (int i = 0; i < _points.Count; i++)
                _tempPoints.Add(transform.InverseTransformPoint(_points[i])); // convert coordinates
            beamCollider.SetPoints(_tempPoints);
            _tempPoints.Clear();
        }

        private void UpdateBeam()
        {
            beam.positionCount = _points.Count;
            for (int i = 0; i < _points.Count; i++)
                beam.SetPosition(i, _points[i]);
        }

        private void ToxicSpread()
        {
            if (originalLightType != LightType.Default) return;
            lightType = _toxicCount > 0 ? LightType.Toxic : LightType.Default;
            _toxicCount = 0;
        }

        private readonly List<RaycastHit2D> _raycastHits = new();

        public void EmitterFixedUpdate()
        {
            if (!beam) return;

            if (lightType == LightType.Solid)
                beamCollider.isTrigger = false;

            _points.Clear();

            // Check for light being enabled
            beamCollider.enabled = _isLightEnabled;
            
            // Toxic spread
            ToxicSpread();

            // Cast beam:
            // I do this on the FixedUpdate because even when deactivated it's still visible

            // Init first cast
            Vector2 origin = transform.position;
            Vector2 direction = transform.right;

            _points.Add(origin);

            RaycastHit2D hit;
            for (int i = 0; i < MaxBeamPoints; i++) // avoid infinite loops
            {
                // get hits
                int hitCount = Physics2D.Raycast(origin, direction, _filter, _raycastHits, MaxDistance);
                int firstValidIndex = FirstValidHit(hitCount);
                if (firstValidIndex < 0)
                {
                    _points.Add(origin + direction * MaxDistance);
                    break;
                }

                // Wall hit: add point
                hit = _raycastHits[firstValidIndex];
                _points.Add(hit.point);

                if (!hit.collider.CompareTag("Mirror")) break;

                origin = hit.point +
                         hit.normal * Epsilon; // move it slightly so that the ray doesn't hit the same thing again
                direction = Vector2.Reflect(direction, hit.normal);
            }

            UpdateBeamCollider();
        }

        public void EmitterUpdate()
        {
            Color color = LightColors[lightType];
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