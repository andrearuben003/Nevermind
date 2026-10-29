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
        public event Action<MemoryKey> RememberedMemory;
        /// Event invoked when a memory is forgotten. Passes the forgotten memory key.
        public event Action<MemoryKey> ForgotMemory;
        /// Event invoked when a memory is forgotten without spawning a thought bubble. Passes the forgotten memory key.
        public event Action<Memory> EliminatedMemory;

        [SerializeField, Range(2, 10)]
        private int memorySlots = 3;
        [SerializeField, Tooltip("The memories the player currently remembers, add entries in the inspector to give them to the player at start.")]
        private List<Memory> memories = new();
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
            foreach (Memory memory in memories)
                RememberedMemory?.Invoke(memory.Key);
            
            foreach (MemoryKey key in Enum.GetValues(typeof(MemoryKey)))
            {
                if (memories.Exists(m => m.Key == key)) continue;
                
                ForgotMemory?.Invoke(key);
            }
        }

        /// Adds a memory to the remembered list if not already present and if there are available slots or upgrades the present memory if of an inferior type.
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
                    EliminatedMemory?.Invoke(mem);
                }                    
                return;
            }
            
            Debug.Log("Remembering memory: " + memory);
            
            memories.Add(memory);
            RememberedMemory?.Invoke(memory.Key);
        }

        /// Removes a memory from the remembered list if present.
        /// <param name="key">The memory key to forget.</param>
        /// <param name="spawnBubble">If true, a thought bubble object will be instantiated.</param>
        public void Forget(MemoryKey key, bool spawnBubble = true)
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
            
            if (spawnBubble)
                spawnThoughtBubble(mem);
            else
                EliminatedMemory?.Invoke(mem);
        }
        
        private void spawnThoughtBubble(Memory memory)
        {
            GameObject bubble = Instantiate(thoughtBubblePrefab, transform.position, Quaternion.identity);
            bubble.GetComponent<ThoughtBubble>().Initialize(memory);
        }
        
        public List<Memory> GetMemories() => memories.ToList();
        
        /// <param name="typeFlags">The memory types that should be included in the result.</param>
        /// <returns>A list of the memories the player remembers that match the desired type.</returns>
        public List<Memory> GetMemoriesByType(MemoryType typeFlags)
        {
            return memories.Where(m => typeFlags.HasFlag(m.Type)).ToList();
        }

        /// Makes the player forget all their current memories and replaces them with new ones.
        /// <param name="newMemories">The memories the player should have.</param>
        public void SetMemories(List<Memory> newMemories)
        {
            foreach (Memory memory in memories.ToList())
                Forget(memory.Key, false);

            foreach (Memory memory in newMemories)
                Remember(memory);
        }
    }
}
