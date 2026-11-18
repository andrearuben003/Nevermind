using Nevermind.Interactables;
using UnityEngine;
using UnityEngine.InputSystem;

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
            Vector2 origin = new(transform.position.x + offset.y * transform.localScale.x, transform.position.y + offset.y);
            
            RaycastHit2D hit = Physics2D.CircleCast(origin, radius, Vector2.zero, 0, interactableLayer);
            
            Interactable interactable = hit.collider ? hit.collider.GetComponent<Interactable>() : null;
            
            if (interactable != selectedInteractable)
            {
                selectedInteractable?.Deselect();
                selectedInteractable =  interactable;
                selectedInteractable?.Select();
            }
        }
        
        public void TryInteract(InputAction.CallbackContext context)
        {
            if (!selectedInteractable) return;

            if (context.started)
                selectedInteractable.Interact();
            
            if (context.canceled && selectedInteractable.ContinuousInteraction)
                selectedInteractable.Interact();
        }
    }
}
