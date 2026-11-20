using UnityEngine;
using UnityEngine.Events;

namespace Nevermind.Lights
{
    [SelectionBase, RequireComponent(typeof(Collider2D))]
    public class LightReceiver : MonoBehaviour, 
        ISolidLightTrigger, IToxicLightTrigger, IDefaultLightTrigger
    {
        public UnityEvent OnActivate;
        public UnityEvent OnDeactivate;

        private bool prevHit;
        private bool hit;
        
        private void FixedUpdate()
        {
            if (!prevHit && hit)
                OnActivate?.Invoke();
            else if (prevHit && !hit)
                OnDeactivate?.Invoke();
            
            prevHit = hit;
            hit = false;
        }

        public void OnSolidLightBeamHit() => onLightHit();
        public void OnToxicLightBeamHit() => onLightHit();
        public void OnDefaultLightBeamHit() => onLightHit();
        private void onLightHit() => hit = true;
    }
}