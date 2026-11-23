using System.Collections.Generic;
using UnityEngine;

namespace Nevermind.MemorySystem
{
    [SelectionBase]
    public class MemoryScreen : MonoBehaviour
    {
        private TagHandle playerTag;

        private void Awake()
        {
            playerTag = TagHandle.GetExistingTag("Player");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;

            List<Memory> mems = MemoryManager.Instance.GetMemories();
            foreach (Memory mem in mems)
                MemoryManager.Instance.Forget(mem, false);
            
        }
    }
}
