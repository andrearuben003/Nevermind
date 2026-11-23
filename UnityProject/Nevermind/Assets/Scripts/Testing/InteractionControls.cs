using Nevermind.Interactables;
using UnityEngine;

namespace Nevermind.Testing
{
    [RequireComponent(typeof(Interactable))]
    public class InteractionControls : MonoBehaviour
    {
        private Interactable interactable;

        private void Awake()
        {
            interactable = GetComponent<Interactable>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                interactable.Interact();
        }
    }
}
