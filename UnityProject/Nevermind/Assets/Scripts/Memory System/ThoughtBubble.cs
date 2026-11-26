using System;
using Nevermind.LevelSystem;
using Nevermind.Lights;
using UnityEngine;

namespace Nevermind.MemorySystem
{
    [SelectionBase]
    public class ThoughtBubble : Killable, IToxicLightTrigger
    {
        /// Event invoked when a thought bubble is destroyed without being collected by the player. Passes the bubble's memory.
        public static event Action<Memory> BubblePopped;
        public event Action<Memory> MemoryChanged;
        
        [SerializeField]
        private bool collectable = true;
        [SerializeField]
        private Memory memory;
        
        private TagHandle playerTag;
        
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
            Destroy(gameObject);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if  (!other.gameObject.CompareTag(playerTag)) return;
            
            collectable = true;
        }

        public override void Kill()
        {
            BubblePopped?.Invoke(memory);
            base.Kill();
        }

        public void OnToxicLightBeamHit()
        {
            // TODO: memory should stay deleted as long as the beam is hitting it
            Kill();
        }
    }
}