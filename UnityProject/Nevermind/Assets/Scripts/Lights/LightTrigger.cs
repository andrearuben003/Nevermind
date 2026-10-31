using UnityEngine;

namespace Nevermind.Lights
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D))]
    public class LightTrigger : MonoBehaviour, 
        ISolidLightTrigger, IToxicLightTrigger, IDefaultLightTrigger, IGravityLightTrigger
    {
        [SerializeField, Tooltip("Trigger sprite")]
        private SpriteRenderer triggerSprite;

        private bool _hit = false;
        
        private void FixedUpdate()
        {
            triggerSprite.color = _hit ? Color.green : Color.red;
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

        public void OnGravityLightBeamHit()
        {
            _hit = true;
        }
    }
}