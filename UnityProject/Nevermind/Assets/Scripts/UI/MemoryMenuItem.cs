using Nevermind.MemorySystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Nevermind.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class MemoryMenuItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        private MemoryGraphicsSettingsSO memoryGraphicsSettings;
        [SerializeField]
        private Image memoryIcon;
        [SerializeField]
        private Image memoryBackground;

        public Memory Memory { get; private set; }
        
        private RectTransform rectTransform;
        private RectTransform containerRect;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            containerRect = rectTransform.parent.GetComponent<RectTransform>();
        }

        public void Initialize(Memory memory)
        {
            Memory = memory;
            memoryIcon.sprite = memoryGraphicsSettings.MemoryIcons[memory];
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            rectTransform.SetParent(transform.root);
            rectTransform.SetAsLastSibling();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(containerRect, Input.mousePosition))
                rectTransform.SetParent(containerRect);
            else
                MemoryManager.Instance.Forget(Memory);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }
    }
}
