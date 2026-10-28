using System.Collections.Generic;
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
        private Color permanentColor;
        [SerializeField]
        private Color semipermanentColor;
        [SerializeField]
        private Color fragileColor;

        private readonly Dictionary<MemoryType, Color> memoryColors = new();

        private void Awake()
        {
            bubble.MemoryChanged += onMemoryChanged;
            
            memoryColors.Add(MemoryType.Permanent, permanentColor);
            memoryColors.Add(MemoryType.SemiPermanent, semipermanentColor);
            memoryColors.Add(MemoryType.Fragile, fragileColor);
        }

        private void onMemoryChanged(Memory memory)
        {
            text.text = memory.Key.ToString();
            spriteRenderer.color = memoryColors[memory.Type];
        }
    }
}

