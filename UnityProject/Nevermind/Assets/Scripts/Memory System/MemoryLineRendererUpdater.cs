using UnityEngine;

namespace Nevermind.MemorySystem
{
    [AddComponentMenu("Memory System/Memory Line Renderer Updater")]
    public class MemoryLineRendererUpdater : SpecificMemoryListener
    {
        private const float ActiveAlpha = 1f;
        
        [SerializeField]
        private LineRenderer lineRenderer;
        [SerializeField]
        private float inactiveAlpha = 0.2f;

        protected override void onRememberedKey()
        {
            Color sColor = lineRenderer.startColor;
            Color eColor = lineRenderer.endColor;
            sColor.a = ActiveAlpha;
            eColor.a = ActiveAlpha;
            lineRenderer.startColor = sColor;
            lineRenderer.endColor = eColor;
        }

        protected override void onForgotKey()
        {
            Color sColor = lineRenderer.startColor;
            Color eColor = lineRenderer.endColor;
            sColor.a = inactiveAlpha;
            eColor.a = inactiveAlpha;
            lineRenderer.startColor = sColor;
            lineRenderer.endColor = eColor;
        }
    }
}
