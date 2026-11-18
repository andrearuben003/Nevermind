using System;
using UnityEngine;

namespace Nevermind.Interactables
{
    [SelectionBase]
    public abstract class Interactable : MonoBehaviour
    {
        public event Action Selected;
        public event Action Deselected;
        
        public abstract void Interact();

        public void Select() => Selected?.Invoke();
        public void Deselect() => Deselected?.Invoke();
    }
}
