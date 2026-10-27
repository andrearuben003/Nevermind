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

        // public void OnTriggerEnter2D(Collider2D other)
        // {
        //     if (!other.gameObject.CompareTag("Light")) return;
        //
        //     triggerSprite.color = Color.green;
        // }
        //
        // public void OnTriggerExit2D(Collider2D other)
        // {
        //     if (!other.gameObject.CompareTag("Light")) return;
        //
        //     triggerSprite.color = Color.red;
        // }
        //
        // // Colliders and triggers both activate the trigger
        // public void OnCollisionEnter2D(Collision2D other)
        // {
        //     OnTriggerEnter2D(other.collider);
        // }
        //
        // public void OnCollisionExit2D(Collision2D other)
        // {
        //     OnTriggerExit2D(other.collider);
        // }

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
            if (_hit)
                triggerSprite.color = Color.green;
            else
                triggerSprite.color = Color.red;
            _hit = false;
        }
    }
}