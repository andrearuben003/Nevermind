using System;
using UnityEngine;

namespace Nevermind.Lights
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D))]
    public class LightTrigger : MonoBehaviour, ISolidLightTrigger, IToxicLightTrigger, IDefaultLightTrigger
    {
        [SerializeField, Tooltip("Trigger sprite")]
        private SpriteRenderer triggerSprite;

        private bool _hit = false;

        public void OnSolidLightBeamHit(Vector2 direction)
        {
            _hit = true;
        }

        public void OnToxicLightBeamHit()
        {
            _hit = true;
        }

        public void OnDefaultLightBeamHit()
        {
            _hit = true;
        }

        private void FixedUpdate()
        {
            triggerSprite.color = _hit ? Color.green : Color.red;
            _hit = false;
        }
    }
}