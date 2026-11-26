using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Nevermind.Lights
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D))]
    public class LightTrigger : LightTriggerTarget, 
        ISolidLightTrigger, IToxicLightTrigger, IDefaultLightTrigger
    {
        [SerializeField, Tooltip("Trigger sprite")]
        private SpriteRenderer triggerSprite;

        [SerializeField] private List<LightTriggerTarget> targets = new();

        private bool _hit = false;
        private bool _lastHit = false;
        
        private void FixedUpdate()
        {
            triggerSprite.color = _hit ? Color.green : Color.red;
            
            // Note that FixedUpdate is always called in the interfaces before it called here,
            // so this should hold
            foreach (LightTriggerTarget target in targets)
                target.OnLightTriggerHit();
            foreach (LightTriggerTarget target in targets)
                if (!_lastHit && _hit)
                    target.OnLightTriggerJustHit();
            foreach (LightTriggerTarget target in targets)
                if (_lastHit && !_hit)
                    target.OnLightTriggerJustStoppedBeingHit();
            
            _lastHit = _hit;
            _hit = false;
        }

        public void OnSolidLightBeamHit()
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
    }
}