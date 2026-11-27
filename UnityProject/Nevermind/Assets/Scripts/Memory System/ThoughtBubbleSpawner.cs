using Nevermind.LevelSystem;
using UnityEditor;
using UnityEngine;

namespace Nevermind.MemorySystem
{
    public class ThoughtBubbleSpawner : Spawner
    {
        [SerializeField]
        private Memory memory;
        
        private void OnEnable()
        {
            MemoryManager.Instance.EliminatedMemory += onBubbleLost;
            ThoughtBubble.BubblePopped += onBubbleLost;
        }

        private void OnDisable()
        {
            MemoryManager.Instance.EliminatedMemory -= onBubbleLost;
            ThoughtBubble.BubblePopped -= onBubbleLost;
        }

        private void onBubbleLost(Memory bubbleMemory)
        {
            if (bubbleMemory == memory)
                spawn();
        }
        
        protected override void spawn()
        {
            GameObject bubble = Instantiate(Prefab, transform.position, Quaternion.identity);
            bubble.transform.parent = transform;
            bubble.GetComponent<ThoughtBubble>().Initialize(memory, true);
        }

        private void OnDrawGizmos()
        {
            GUIStyle style = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                normal =
                {
                    textColor = Color.magenta
                }
            };
            Handles.Label(transform.position + Vector3.down * 0.5f, new GUIContent(memory.ToString()), style);
        }
    }
}
