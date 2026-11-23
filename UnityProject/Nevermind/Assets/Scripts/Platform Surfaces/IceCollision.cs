using UnityEngine;

namespace Nevermind.PlatformSurfaces
{
    public static class IceCollision
    {
        public static void TransferMomentum(IIceAffected self, Collision2D other)
        {
            float dir = self.SlideDir;

            if (dir == 0f || !self.IsOnIce)
                return;
            
            IIceAffected otherIce = other.gameObject.GetComponent<IIceAffected>();

            if (!otherIce.IsOnIce || otherIce.SlideDir != 0f)
                return;

            if (Mathf.Abs(other.GetContact(0).normal.x) < 0.8f)
                return;
            
            float dist = other.transform.position.x - other.otherCollider.transform.position.x;

            if (Mathf.Sign(dist) != dir)
                return;
            
            self.StopSliding();
            otherIce.StartSliding(dir);
        }
    }
}