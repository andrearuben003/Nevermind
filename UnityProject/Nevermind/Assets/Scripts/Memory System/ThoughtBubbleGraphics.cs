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
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private TextMeshProUGUI text;
        [Space]
        [SerializeField]
        private MemoryGraphicsSettingsSO settings;
        
        private void Awake()
        {
            bubble.MemoryChanged += onMemoryChanged;
        }

        private void onMemoryChanged(Memory memory)
        {
            text.text = memory.Key.ToString();
            Color c = settings.TypeColors[memory.Type];
            c.a = 0.3f;
            spriteRenderer.color = c;
        }
    }
}

