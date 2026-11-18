using Nevermind.Interactables;
using UnityEngine;

namespace Nevermind.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField, Min(0)]
        private float radius;

        [SerializeField]
        private Vector2 offset;
        
        [SerializeField, Min(0)]
        private float checkInterval;
        
        [SerializeField]
        private LayerMask interactableLayer;
        
        private Interactable selectedInteractable;

        private void Start()
        {
            InvokeRepeating(nameof(checkInteractionRange), 0f, checkInterval);
        }

        private void checkInteractionRange()
        {
            Vector2 origin = new  Vector2(transform.position.x + offset.y * transform.localScale.x, transform.position.y + offset.y);
            
            RaycastHit2D hit = Physics2D.CircleCast(origin, radius, Vector2.zero, 0, interactableLayer);
            
            selectedInteractable?.Deselect();
            selectedInteractable = hit.collider ? hit.collider.GetComponent<Interactable>() : null;
            selectedInteractable?.Select();
        }
        
        public void TryInteract()
        {
            selectedInteractable?.Interact();
        }
    }
}
