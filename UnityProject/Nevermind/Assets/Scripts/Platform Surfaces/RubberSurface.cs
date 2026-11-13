using System;
using System.Collections.Generic;
using Nevermind.MemorySystem;
using Nevermind.Player.Controller;
using TarodevController;
using UnityEngine;

namespace Nevermind.PlatformSurfaces
{
    public class RubberSurface : MonoBehaviour
    {
        /// <summary>
        /// Bounce Speed for objects (as boxes) and Tarodev Player Controller
        /// </summary>
        [SerializeField] float bounceSpeed = 10;
        
        /// <summary>
        /// Bounce Speed for GMTK Player Controller
        /// </summary>
        [SerializeField] float bounceForce = 10;

        private Rigidbody2D _rb;

        void Awake()
        {
            _rb = this.gameObject.GetComponentInParent<Rigidbody2D>();
        }

        void OnEnable()
        {
            List<Collider2D> cols = new();
            GetComponent<Collider2D>().Overlap(cols);

            foreach (Collider2D col in cols) {
                GenerateBounce(col);
            }

        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!enabled)
                return;
        
            GenerateBounce(other);
        }

        private void GenerateBounce(Collider2D other)
        {
            if (other.attachedRigidbody == _rb || other.attachedRigidbody == null)
                return;
        
            Vector2 velocity = this.transform.up * bounceSpeed;
        
            if(_rb.bodyType == RigidbodyType2D.Dynamic)
                _rb.linearVelocity = -velocity;
        
            if(other.gameObject.CompareTag("Player"))
            {
                if (other.gameObject.TryGetComponent<PlayerController>(out var pl))
                    pl.Bounce(velocity);
                else if (other.gameObject.TryGetComponent<PlatformerController>(out var pc))
                    pc.Bounce(this.transform.up, bounceForce);
            }
            else if(other.attachedRigidbody.bodyType == RigidbodyType2D.Dynamic)
                other.attachedRigidbody.linearVelocity = velocity;
        }
    }
}