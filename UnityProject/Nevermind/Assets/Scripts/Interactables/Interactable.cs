using System;
using UnityEngine;

namespace Nevermind.Interactables
{
    [SelectionBase]
    public abstract class Interactable : MonoBehaviour
    {
        public event Action Selected;
        public event Action Deselected;
        
        public virtual void Interact() {}
        public virtual void InteractHoldStart() => Interact();
        public virtual void InteractHoldEnd() => Interact();

        public virtual void Select() => Selected?.Invoke();
        public virtual void Deselect() => Deselected?.Invoke();
    }
}
