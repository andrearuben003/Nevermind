using Nevermind.UI;
using UnityEngine;

namespace Nevermind.MemorySystem
{
    public class ThoughtBubbleGraphics : MonoBehaviour
    {
        [SerializeField]
        private ThoughtBubble bubble;
        [SerializeField]
        private SpriteRenderer iconSprite;
        [Space]
        [SerializeField]
        private MemoryGraphicsSettingsSO settings;
        
        private void Awake()
        {
            bubble.MemoryChanged += onMemoryChanged;
        }

        private void onMemoryChanged(Memory memory)
        {
            iconSprite.sprite = settings.MemoryIcons[memory];
        }
    }
}

