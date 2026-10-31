namespace Nevermind.Lights
{
    public interface IGravityLightTrigger
    {
        /** Called when a toxic beam hits this. This is checked in a FixedUpdate */
        void OnGravityLightBeamHit();
    }
}