using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nevermind.MemorySystem
{
    public class MemoryManager : MonoBehaviour
    {
        public static MemoryManager Instance;

        /// Event invoked when a memory is remembered. Passes the remembered memory key.
        public event Action<MemoryKey> RememberedMemory;
        /// Event invoked when a memory is forgotten. Passes the forgotten memory key.
        public event Action<MemoryKey> ForgotMemory;

        [SerializeField, Range(2, 10)]
        private int memorySlots = 3;
        [SerializeField, Tooltip("The memories the player currently remembers, add entries in the inspector to give them to the player at start.")]
        private List<Memory> memories = new();
        
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
            foreach (Memory memory in memories)
                RememberedMemory?.Invoke(memory.Key);

            foreach (MemoryKey key in Enum.GetValues(typeof(MemoryKey)))
            {
                if (memories.Exists(m => m.Key == key)) continue;
                
                ForgotMemory?.Invoke(key);
            }
        }

        /// Adds a memory to the remembered list if not already present and if there are available slots and invokes RememberedMemory.
        /// <param name="memory">The memory to remember.</param>
        public void Remember(Memory memory)
        {
            if (memories.Exists(m => m.Key == memory.Key) || memories.Count == memorySlots) return;
            
            Debug.Log("Remembering memory: " + memory);
            
            memories.Add(memory);
            RememberedMemory?.Invoke(memory.Key);
        }

        /// Removes a memory from the remembered list if present and invokes ForgotMemory.
        /// <param name="memory">The memory to forget.</param>
        public void Forget(Memory memory)
        {
            if (!memories.Contains(memory)) return;
            
            Debug.Log("Forgetting memory: " + memory);
            
            memories.Remove(memory);
            ForgotMemory?.Invoke(memory.Key);
        }
    }
}
