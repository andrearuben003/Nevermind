using System;

namespace Nevermind.Lights
{
    [Obsolete("Gravity logic has been moved to all the beams. This interface")]
    public interface IGravityLightTrigger
    {
        /** Called when a toxic beam hits this. This is checked in a FixedUpdate */
        void OnGravityLightBeamHit();
    }
}