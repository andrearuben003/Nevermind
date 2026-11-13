using System;
using System.Collections.Generic;
using Nevermind.Player.Controller;
using UnityEngine;

namespace Nevermind.PlatformSurfaces
{
    public class IceSurface2 : MonoBehaviour
    {

        private Collider2D _collider;
        private readonly Dictionary<Collider2D, IIceAffected> _entities = new();

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }
        
        void OnTriggerEnter2D(Collider2D other)
        {
            IceEffectEnable(other);
        }
        
        void OnTriggerExit2D(Collider2D other)
        {
            IceEffectDisable(other);
        }

        void OnEnable()
        {//Check for any colliders already on the surface
            
            List<Collider2D> cols = new();
            _collider.Overlap(cols);

            foreach (Collider2D col in cols) {
                IceEffectEnable(col);
            }
        }

        void OnDisable()
        {//Check for are any colliders still on the surface
            
            List<Collider2D> cols = new();
            GetComponent<Collider2D>().Overlap(cols);

            foreach (Collider2D col in cols) {
                IceEffectDisable(col);
            }
        }
        
        private void IceEffectEnable(Collider2D other)
        {
            if(other.gameObject.TryGetComponent<IIceAffected>(out IIceAffected iceAffected))
            {
                //other.gameObject.GetComponent<PlatformerController>().SetOnIce(true);
                _entities.Add(other, iceAffected);
                iceAffected.EnterIce();
            }
        }

        private void IceEffectDisable(Collider2D other)
        {
            if(_entities.Remove(other, out var iceAffected))
            {
                iceAffected.ExitIce();
            }
        }
    }
    
    
}
