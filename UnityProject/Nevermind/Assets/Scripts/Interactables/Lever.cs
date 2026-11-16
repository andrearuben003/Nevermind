using UnityEngine;
using UnityEngine.Events;

namespace Nevermind.Interactables
{
    public class Lever : Interactable
    {
        public UnityEvent OnActivated = new();
        public UnityEvent OnDeactivated = new();

        private bool isActivated;

        protected override void onSuccessfulInteraction()
        {
            if (!isActivated)
                OnActivated.Invoke();
            else
                OnDeactivated.Invoke();

            isActivated = !isActivated;
        }
    }
}
