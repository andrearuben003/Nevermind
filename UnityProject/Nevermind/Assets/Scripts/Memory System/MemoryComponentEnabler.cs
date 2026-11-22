using UnityEngine;

namespace Nevermind.MemorySystem
{
    [AddComponentMenu("Memory System/Memory Component Enabler")]
    public class MemoryComponentEnabler : SpecificMemoryListener
    {
        [SerializeField, Tooltip("The components that will be enabled/disabled when the correct memory is remembered/forgotten.")]
        private Behaviour[] components;

        protected override void onRememberedKey()
        {
            foreach (Behaviour comp in components)
                comp.enabled = true;
        }

        protected override void onForgotKey()
        {
            foreach (Behaviour comp in components)
                comp.enabled = false;
        }
    }
}
