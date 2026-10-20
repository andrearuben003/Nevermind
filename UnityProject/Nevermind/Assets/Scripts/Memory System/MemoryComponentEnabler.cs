using UnityEngine;

namespace Nevermind.MemorySystem
{
    [AddComponentMenu("Memory System/Memory Component Enabler")]
    public class MemoryComponentEnabler : MemoryListener
    {
        [SerializeField, Tooltip("The components that will be enabled/disabled when the correct memory is remembered/forgotten.")]
        private Behaviour[] components;

        protected override void onRemembered(MemoryKey key)
        {
            if (key != MemoryKey) return;
            
            foreach (Behaviour comp in components)
                comp.enabled = true;
        }

        protected override void onForgot(MemoryKey key)
        {
            if (key != MemoryKey) return;
            
            foreach (Behaviour comp in components)
                comp.enabled = false;
        }
    }
}
