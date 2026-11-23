using UnityEngine;

namespace Nevermind.MemorySystem
{
    public class MemoryFont : MonoBehaviour
    {
        [SerializeField]
        private Memory[] restorableMemories;

        private TagHandle playerTag;

        private void Awake()
        {
            playerTag = TagHandle.GetExistingTag("Player");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;

            foreach (Memory key in restorableMemories)
                MemoryManager.Instance.Remember(key);
        }
    }
}
