using UnityEditor;
using UnityEngine;

namespace Nevermind.MemorySystem
{
    [SelectionBase]
    public class ThoughtBubbleSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject bubblePrefab;

        [SerializeField]
        private Memory memory;

        private void Awake()
        {
            spawnBubble();
        }

        private void OnEnable()
        {
            MemoryManager.Instance.EliminatedMemory += onBubbleLost;
            ThoughtBubble.ThoughtBubbleLost += onBubbleLost;
        }

        private void OnDisable()
        {
            MemoryManager.Instance.EliminatedMemory -= onBubbleLost;
            ThoughtBubble.ThoughtBubbleLost -= onBubbleLost;
        }

        private void onBubbleLost(Memory memory)
        {
            if (memory == this.memory)
                spawnBubble();
        }
        
        private void spawnBubble()
        {
            GameObject bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity);
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
