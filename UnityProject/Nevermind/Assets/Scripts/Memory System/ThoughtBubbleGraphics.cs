using Nevermind.UI;
using TMPro;
using UnityEngine;

namespace Nevermind.MemorySystem
{
    public class ThoughtBubbleGraphics : MonoBehaviour
    {
        [SerializeField]
        private ThoughtBubble bubble;
        [SerializeField]
        private SpriteRenderer backgroundSprite;
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
            Color c = settings.TypeColors[memory.Type];
            c.a = 0.3f;
            backgroundSprite.color = c;
            
            iconSprite.sprite = settings.MemoryIcons[memory.Key];
        }
    }
}

