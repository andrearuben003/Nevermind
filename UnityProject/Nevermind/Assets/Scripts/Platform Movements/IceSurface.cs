using System;
using System.Collections.Generic;
using Nevermind.MemorySystem;
using TarodevController;
using UnityEngine;

namespace Nevermind.PlatformMovements
{
    public class IceSurface : MonoBehaviour
    {

        [SerializeField] private PhysicsMaterial2D iceMaterial;
        [SerializeField] private PhysicsMaterial2D platformMaterial;
        private Collider2D _collider;
        private HashSet<Collider2D> _playerCollidersOnSurface;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _playerCollidersOnSurface = new HashSet<Collider2D>();
        }

        void OnEnable()
        {//Check for any colliders already on the surface
            _collider.sharedMaterial = iceMaterial;
            
            List<Collider2D> cols = new();
            _collider.Overlap(cols);

            foreach (Collider2D col in cols) {
                IceEffectEnable(col);
            }
        }

        void OnDisable()
        {//Check for are any colliders still on the surface
            _collider.sharedMaterial = platformMaterial;
            
            List<Collider2D> cols = new();
            GetComponent<Collider2D>().Overlap(cols);

            foreach (Collider2D col in cols) {
                IceEffectDisable(col);
            }
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!enabled)
                return;
            
            IceEffectEnable(collision.collider);
        }
        
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!enabled)
                return;
            
            IceEffectDisable(collision.collider);
        }

        private void IceEffectEnable(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _playerCollidersOnSurface.Add(other);
                other.gameObject.GetComponent<PlayerController>().SetIce(true);
            }
        }

        private void IceEffectDisable(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _playerCollidersOnSurface.Remove(other);
                if(_playerCollidersOnSurface.Count == 0)
                    other.gameObject.GetComponent<PlayerController>().SetIce(false);
            }
            
        }
    }
}
