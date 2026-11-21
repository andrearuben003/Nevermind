using UnityEngine;

namespace Nevermind.MemorySystem
{
    [AddComponentMenu("Memory System/Memory Sprite Updater")]
    public class MemorySpriteUpdater : SpecificMemoryListener
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField, Range(0f, 1f)]
        private float activeAlpha = 1f;
        [SerializeField, Range(0f, 1f)]
        private float inactiveAlpha = 0.2f;
        
        protected override void onRememberedKey()
        {
            Color color = spriteRenderer.color;
            color.a = activeAlpha;
            spriteRenderer.color = color;
        }

        protected override void onForgotKey()
        {
            Color color = spriteRenderer.color;
            color.a = inactiveAlpha;
            spriteRenderer.color = color;
        }
    }
}
