using System;
using UnityEngine;

namespace Nevermind.Interactables
{
    [SelectionBase]
    public abstract class Interactable : MonoBehaviour
    {
        /// If true, Interact will be called also when the button is released.
        public bool ContinuousInteraction { get; protected set; }
        public event Action Selected;
        public event Action Deselected;
        
        public abstract void Interact();

        public virtual void Select() => Selected?.Invoke();
        public virtual void Deselect() => Deselected?.Invoke();
    }
}
