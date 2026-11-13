using UnityEngine;
using UnityEngine.Tilemaps;

namespace Nevermind.MemorySystem
{
    [AddComponentMenu("Memory System/Memory Tile Updater")]
    public class MemoryTileUpdater : MemoryListener
    {
        [SerializeField]
        private Tilemap tileMap;

        [SerializeField, Range(0f, 1f)]
        private float activeAlpha = 1f;

        [SerializeField, Range(0f, 1f)]
        private float inactiveAlpha = 0.2f;

        protected override void onRemembered(MemoryKey key)
        {
            if (key != MemoryKey) return;

            Color color = tileMap.color;
            color.a = activeAlpha;
            tileMap.color = color;
        }

        protected override void onForgot(MemoryKey key)
        {
            if (key != MemoryKey) return;

            Color color = tileMap.color;
            color.a = inactiveAlpha;
            tileMap.color = color;
        }
    }
}
