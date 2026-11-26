using UnityEngine;

namespace Nevermind.Lights
{
    public class LightTriggerTarget : MonoBehaviour
    {
        public virtual void OnLightTriggerHit() {}

        public virtual void OnLightTriggerJustHit() {}

        public virtual void OnLightTriggerJustStoppedBeingHit() {}
    }
}