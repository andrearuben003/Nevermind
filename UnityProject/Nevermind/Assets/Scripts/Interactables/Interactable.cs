using System;
using UnityEngine;

namespace Nevermind.Interactables
{
    [RequireComponent(typeof(Collider2D)), SelectionBase]
    public abstract class Interactable : MonoBehaviour
    {
        public event Action playerEnteredInteractionRange;
        public event Action playerExitedInteractionRange;

        private bool playerInRange;
        private TagHandle playerTag;

        protected void Awake()
        {
            playerTag = TagHandle.GetExistingTag("Player");
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;
            
            playerInRange = true;
            playerEnteredInteractionRange?.Invoke();
        }

        protected void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;
            
            playerInRange = false;
            playerExitedInteractionRange?.Invoke();
        }

        public void Interact()
        {
            if (!playerInRange) return;
            
            Debug.Log($"Interacted with {name}");
            onSuccessfulInteraction();
        }

        protected abstract void onSuccessfulInteraction();
    }
}
