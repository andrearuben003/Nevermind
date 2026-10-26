using UnityEngine;

namespace Nevermind.MemorySystem
{
    [AddComponentMenu("Memory System/Memory Sprite Updater")]
    public class MemorySpriteUpdater : MemoryListener
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private float inactiveAlpha = 0.2f;

        [SerializeField]
        private float activeAlpha = 1f;
        
        protected override void onRemembered(MemoryKey key)
        {
            if (key != MemoryKey) return;
            
            Color color = spriteRenderer.color;
            color.a = activeAlpha;
            spriteRenderer.color = color;
        }

        protected override void onForgot(MemoryKey key)
        {
            if (key != MemoryKey) return;
            
            Color color = spriteRenderer.color;
            color.a = inactiveAlpha;
            spriteRenderer.color = color;
        }
    }
}
