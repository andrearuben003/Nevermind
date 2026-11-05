namespace Nevermind.Lights
{
    public interface ISolidLightTrigger
    {
        /** Called when a solid beam hits this. This is checked in a FixedUpdate */
        void OnSolidLightBeamHit();
    }
}