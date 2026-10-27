using UnityEngine;

namespace Nevermind.Lights
{
    public interface IToxicLightTrigger
    {
        /** Called when a toxic beam hits this. This is checked in a FixedUpdate */
        void OnToxicLightBeamHit();
    }
}