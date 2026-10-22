using UnityEngine;

namespace Nevermind.MemorySystem
{
    public class MemoryFont : MonoBehaviour
    {
        [SerializeField]
        private MemoryKey[] restorableMemories;

        private TagHandle playerTag;

        private void Awake()
        {
            playerTag = TagHandle.GetExistingTag("Player");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;

            foreach (MemoryKey key in restorableMemories)
                MemoryManager.Instance.Remember(new Memory(key, MemoryType.SemiPermanent));
        }
    }
}
