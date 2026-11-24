using System.Collections.Generic;
using Nevermind.MemorySystem;
using UnityEngine;
using UnityEngine.InputSystem;
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
                selectedIndex = Mathf.Clamp(value, 0, Mathf.Clamp(menuEntries.Count - 1, 0, int.MaxValue));
                if (menuEntries.Count == 0)
                    selector.gameObject.SetActive(false);
                else
                {
                    selector.gameObject.SetActive(true);
                    Canvas.ForceUpdateCanvases();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(memoryEntryParent);
                    
                    selector.position = memoryEntryParent.GetChild(selectedIndex).transform.position;
                }
            }
        }
        
        private Dictionary<Memory, GameObject> menuEntries = new();

        public void Toggle()
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(!child.gameObject.activeSelf);
        }

        public void SelectNext(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            
            if (memoryEntryParent.childCount == 0) return;
            
            SelectedIndex = (SelectedIndex + 1) % memoryEntryParent.childCount;
        }
        
        public void SelectPrevious(InputAction.CallbackContext context) 
        {
            if (!context.started) return;
            
            if (memoryEntryParent.childCount == 0) return;
            
            int r = (SelectedIndex - 1) % memoryEntryParent.childCount;
            SelectedIndex = r < 0 ? r + memoryEntryParent.childCount : r;
        }

        public void ForgetSelected(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            
            if (SelectedIndex < 0 || SelectedIndex >= menuEntries.Count) return;
            
            Memory selectedMem = memoryEntryParent.GetChild(SelectedIndex).GetComponent<MemoryMenuItem>().Memory;
            MemoryManager.Instance.Forget(selectedMem);
        }
        
        protected override void onRemembered(Memory memory)
        {
            if (menuEntries.ContainsKey(memory)) return;
            
            GameObject entry = Instantiate(memoryEntryPrefab, memoryEntryParent);
            entry.GetComponent<MemoryMenuItem>().Initialize(memory);
            
            menuEntries.Add(memory, entry);
            SelectedIndex = selectedIndex;
        }

        protected override void onForgot(Memory memory)
        {
            if (menuEntries.TryGetValue(memory, out GameObject entry))
                Destroy(entry);
            
            menuEntries.Remove(memory);
            SelectedIndex = selectedIndex;
        }
    }
}