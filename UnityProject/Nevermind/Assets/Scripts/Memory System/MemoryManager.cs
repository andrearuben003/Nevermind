using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nevermind.MemorySystem
{
    [AddComponentMenu("Memory System/Memory Manager")]
    public class MemoryManager : MonoBehaviour
    {
        public static MemoryManager Instance;

        /// Event invoked when a memory is remembered. Passes the remembered memory key.
        public event Action<Memory> RememberedMemory;
        /// Event invoked when a memory is forgotten. Passes the forgotten memory key.
        public event Action<Memory> ForgotMemory;
        /// Event invoked when a memory is forgotten without spawning a thought bubble. Passes the forgotten memory key.
        public event Action<Memory> EliminatedMemory;
        
        [SerializeField, Tooltip("The memories the player currently remembers, add entries in the inspector to give them to the player at start.")]
        private List<Memory> rememberedMemories = new();
        [SerializeField]
        private GameObject thoughtBubblePrefab;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        private void Start()
        {
            // Invokes remembered and forgot at start to make sure all components are in the correct starting state
            foreach (Memory memory in rememberedMemories)
                RememberedMemory?.Invoke(memory);
            
            foreach (Memory key in Enum.GetValues(typeof(Memory)))
            {
                if (rememberedMemories.Contains(key)) continue;
                
                ForgotMemory?.Invoke(key);
            }
        }

        /// Adds a memory to the remembered list if not already present.
        /// <param name="memory">The memory to remember.</param>
        public void Remember(Memory memory)
        {
            if (rememberedMemories.Contains(memory)) return;
            
            Debug.Log("Remembering memory: " + memory);
            
            rememberedMemories.Add(memory);
            RememberedMemory?.Invoke(memory);
        }

        /// Removes a memory from the remembered list if present.
        /// <param name="memory">The memory to forget.</param>
        /// <param name="spawnBubble">If true, a thought bubble object will be instantiated.</param>
        public void Forget(Memory memory, bool spawnBubble = true)
        {
            if (!rememberedMemories.Contains(memory)) return;
            
            Debug.Log("Forgetting memory: " + memory);
            
            rememberedMemories.Remove(memory);
            ForgotMemory?.Invoke(memory);
            
            if (spawnBubble)
                spawnThoughtBubble(memory);
            else
                EliminatedMemory?.Invoke(memory);
        }
        
        private void spawnThoughtBubble(Memory memory)
        {
            GameObject bubble = Instantiate(thoughtBubblePrefab, transform.position, Quaternion.identity);
            bubble.GetComponent<ThoughtBubble>().Initialize(memory);
        }
        
        public List<Memory> GetMemories() => rememberedMemories.ToList();

        /// Makes the player forget all their current memories and replaces them with new ones.
        /// <param name="newMemories">The memories the player should have.</param>
        public void SetMemories(List<Memory> newMemories)
        {
            foreach (Memory memory in rememberedMemories.ToList())
                Forget(memory, false);

            foreach (Memory memory in newMemories)
                Remember(memory);
        }
    }
}
