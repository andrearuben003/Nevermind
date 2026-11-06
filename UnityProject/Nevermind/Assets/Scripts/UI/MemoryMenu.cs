using System.Collections.Generic;
using Nevermind.MemorySystem;
using UnityEngine;

namespace Nevermind.UI
{
    [SelectionBase]
    public class MemoryMenu : MemoryListener
    {
        [SerializeField]
        private GameObject memoryEntryPrefab;
        
        [SerializeField]
        private Transform memoryEntryParent;
        
        private Dictionary<MemoryKey, GameObject> menuEntries = new Dictionary<MemoryKey, GameObject>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                foreach (Transform child in transform)
                    child.gameObject.SetActive(!child.gameObject.activeSelf);
            }
        }
        protected override void onRemembered(MemoryKey key)
        {
            GameObject entry = Instantiate(memoryEntryPrefab, memoryEntryParent);
            entry.GetComponent<MemoryMenuItem>().Initialize(getFullMemory(key));
            menuEntries.Add(key, entry);
        }

        protected override void onForgot(MemoryKey key)
        {
            if (menuEntries.TryGetValue(key, out GameObject entry))
                Destroy(entry);
            
            menuEntries.Remove(key);
        }

        private Memory getFullMemory(MemoryKey key)
        {
            return MemoryManager.Instance.GetMemories().Find(m => m.Key == key);
        }
    }
}