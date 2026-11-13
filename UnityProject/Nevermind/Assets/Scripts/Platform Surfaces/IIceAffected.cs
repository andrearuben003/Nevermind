namespace Nevermind.PlatformSurfaces
{
    /// <summary>
    /// Interface for entities that may be affected by Ice Surfaces
    /// </summary>
    public interface IIceAffected
    {
        void EnterIce();
        void ExitIce();
    }
}