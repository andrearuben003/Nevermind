using Nevermind.Interactables;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

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
            Vector2 origin = new(transform.position.x + offset.x * transform.localScale.x, transform.position.y + offset.y);
            
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
            //if (context.interaction is TapInteraction && context.performed)
            //    Debug.Log("Tap");
            //if (context.interaction is HoldInteraction && context.started)
            //    Debug.Log("Hold");
            //if (context.interaction is HoldInteraction && context.performed)
            //    Debug.Log("Hold performed");
            //if (context.interaction is HoldInteraction && context.canceled)
            //    Debug.Log("Hold canceled");
            
            if (!selectedInteractable) return;

            switch (context.interaction)
            {
                case TapInteraction when context.performed:
                    selectedInteractable.Interact();
                    break;
                case HoldInteraction when context.performed:
                    selectedInteractable.InteractHoldStart();
                    break;
                case HoldInteraction when context.canceled:
                    selectedInteractable.InteractHoldEnd();
                    break;
            }
        }
    }
}
