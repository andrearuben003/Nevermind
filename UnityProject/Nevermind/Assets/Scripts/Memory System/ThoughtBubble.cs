using System;
using UnityEngine;

namespace Nevermind.MemorySystem
{
    [SelectionBase]
    public class ThoughtBubble : MonoBehaviour
    {
        /// Event invoked when a thought bubble is destroyed without being collected by the player. Passes the bubble's memory.
        public static event Action<Memory> ThoughtBubbleLost;
        public event Action<Memory> MemoryChanged;
        
        [SerializeField]
        private bool collectable = true;
        [SerializeField]
        private Memory memory;
        
        private TagHandle playerTag;
        private bool wasCollected;
        
        private void Awake()
        {
            playerTag = TagHandle.GetExistingTag("Player");
        }

        private void Start()
        {
            MemoryChanged?.Invoke(memory);
        }

        public void Initialize(Memory memory, bool collectable = false)
        {
            this.memory = memory;
            this.collectable = collectable;
            MemoryChanged?.Invoke(memory);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!collectable || !other.gameObject.CompareTag(playerTag)) return;
            
            MemoryManager.Instance.Remember(memory);
            wasCollected = true;
            Destroy(gameObject);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if  (!other.gameObject.CompareTag(playerTag)) return;
            
            collectable = true;
        }

        private void OnDestroy()
        {
            if (!wasCollected)
                ThoughtBubbleLost?.Invoke(memory);
        }
    }
}