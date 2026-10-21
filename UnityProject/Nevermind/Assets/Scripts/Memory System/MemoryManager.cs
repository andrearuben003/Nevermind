using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nevermind.MemorySystem
{
    [AddComponentMenu("Memory System/Memory Manager")]
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
            if (memories.Count == memorySlots) return;

            if (memories.Exists(m => m.Key == memory.Key))
            {
                Memory mem = memories.Find(m => m.Key == memory.Key);
                if ((int)mem.Type < (int)memory.Type)
                {
                    Debug.Log($"Upgraded memory {mem} to {memory.Type}");
                    memories.Remove(mem);
                    memories.Add(memory);
                    return;
                }
            }
            
            Debug.Log("Remembering memory: " + memory);
            
            memories.Add(memory);
            RememberedMemory?.Invoke(memory.Key);
        }

        /// Removes a memory from the remembered list if present and invokes ForgotMemory.
        /// <param name="key">The memory key to forget.</param>
        public void Forget(MemoryKey key)
        {
            if (!memories.Exists(m => m.Key == key)) return;
            
            Memory mem = memories.Find(m => m.Key == key);
            if (mem.Type == MemoryType.Permanent)
            {
                Debug.Log("Can't forget permanent memory: " + key);
                return;
            }
            
            Debug.Log("Forgetting memory: " + mem);
            
            memories.Remove(mem);
            ForgotMemory?.Invoke(mem.Key);
        }
    }
}
