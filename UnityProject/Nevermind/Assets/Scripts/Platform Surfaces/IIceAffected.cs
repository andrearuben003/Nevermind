namespace Nevermind.PlatformSurfaces
{
    /// <summary>
    /// Interface for entities that may be affected by Ice Surfaces
    /// </summary>
    public interface IIceAffected
    {
        void EnterIce();
        void ExitIce();
        
        bool IsOnIce { get; }
        float SlideDir { get; }
        
        void StartSliding(float dir);
        void StopSliding();
    }
}