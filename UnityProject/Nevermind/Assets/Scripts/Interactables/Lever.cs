using UnityEngine;
using UnityEngine.Events;

namespace Nevermind.Interactables
{
    public class Lever : Interactable
    {
        public UnityEvent OnActivated = new();
        public UnityEvent OnDeactivated = new();

        private bool isActivated;

        public override void Interact()
        {
            if (!isActivated)
                OnActivated.Invoke();
            else
                OnDeactivated.Invoke();

            isActivated = !isActivated;
        }
    }
}
