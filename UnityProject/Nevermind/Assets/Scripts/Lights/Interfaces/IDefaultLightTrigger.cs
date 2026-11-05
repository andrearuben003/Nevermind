namespace Nevermind.Lights
{
    public interface IDefaultLightTrigger
    {
        /** Called when a default beam hits this. This is checked in a FixedUpdate */
        void OnDefaultLightBeamHit();
    }
}