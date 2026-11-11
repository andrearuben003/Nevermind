using System.Collections.Generic;
using Nevermind.MemorySystem;
using UnityEngine;
using UnityEngine.UI;

namespace Nevermind.UI
{
    [SelectionBase]
    public class MemoryMenu : MemoryListener
    {
        [SerializeField]
        private GameObject memoryEntryPrefab;
        
        [SerializeField]
        private RectTransform memoryEntryParent;

        [SerializeField]
        private Transform selector;

        private int selectedIndex;

        private int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                Debug.Log("Setting "  + value + " over " + menuEntries.Count);
                selectedIndex = Mathf.Clamp(value, 0, Mathf.Clamp(menuEntries.Count - 1, 0, int.MaxValue));
                if (menuEntries.Count == 0)
                    selector.gameObject.SetActive(false);
                else
                {
                    selector.gameObject.SetActive(true);
                    Debug.Log(selectedIndex + " Pos "  + memoryEntryParent.GetChild(selectedIndex));
                    Canvas.ForceUpdateCanvases();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(memoryEntryParent);
                    
                    selector.position = memoryEntryParent.GetChild(selectedIndex).transform.position;
                }
            }
        }
        
        private Dictionary<MemoryKey, GameObject> menuEntries = new Dictionary<MemoryKey, GameObject>();

        //private void Start()
        //{
        //    SelectedIndex = -1;
        //}

        public void Toggle()
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(!child.gameObject.activeSelf);
        }

        public void SelectNext()
        {
            if (memoryEntryParent.childCount == 0) return;
            
            SelectedIndex = (SelectedIndex + 1) % memoryEntryParent.childCount;
        }
        
        public void SelectPrevious() 
        {
            if (memoryEntryParent.childCount == 0) return;
            
            int r = (SelectedIndex - 1) % memoryEntryParent.childCount;
            SelectedIndex = r < 0 ? r + memoryEntryParent.childCount : r;
        }

        public void ForgetSelected()
        {
            Memory selectedMem = memoryEntryParent.GetChild(SelectedIndex).GetComponent<MemoryMenuItem>().Memory;
            MemoryManager.Instance.Forget(selectedMem.Key);
        }
        
        protected override void onRemembered(MemoryKey key)
        {
            GameObject entry = Instantiate(memoryEntryPrefab, memoryEntryParent);
            Memory mem = getFullMemory(key);
            entry.GetComponent<MemoryMenuItem>().Initialize(mem);
            
            menuEntries.Add(key, entry);
            SelectedIndex = selectedIndex;
        }

        protected override void onForgot(MemoryKey key)
        {
            if (menuEntries.TryGetValue(key, out GameObject entry))
                Destroy(entry);
            
            menuEntries.Remove(key);
            SelectedIndex = selectedIndex;
        }

        private Memory getFullMemory(MemoryKey key)
        {
            return MemoryManager.Instance.GetMemories().Find(m => m.Key == key);
        }
    }
}