using UnityEngine;

namespace Nevermind.MemorySystem
{
    [AddComponentMenu("Memory System/Memory Line Renderer Updater")]
    public class MemoryLineRendererUpdater : MemoryListener
    {
        [SerializeField]
        private LineRenderer lineRenderer;
        [SerializeField]
        private float inactiveAlpha = 0.2f;
        
        private float activeAlpha = 1f;
        
        protected override void onRemembered(MemoryKey key)
        {
            if (key != MemoryKey) return;
            
            Color sColor = lineRenderer.startColor;
            Color eColor = lineRenderer.endColor;
            sColor.a = activeAlpha;
            eColor.a = activeAlpha;
            lineRenderer.startColor = sColor;
            lineRenderer.endColor = eColor;
        }

        protected override void onForgot(MemoryKey key)
        {
            if (key != MemoryKey) return;
            
            Color sColor = lineRenderer.startColor;
            Color eColor = lineRenderer.endColor;
            sColor.a = inactiveAlpha;
            eColor.a = inactiveAlpha;
            lineRenderer.startColor = sColor;
            lineRenderer.endColor = eColor;
        }
    }
}
