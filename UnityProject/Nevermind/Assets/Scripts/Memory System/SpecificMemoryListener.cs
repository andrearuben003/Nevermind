using UnityEngine;

namespace Nevermind.MemorySystem
{
    /// Base class for MonoBehaviours that want to listen for memory events on a specific memory.
    /// <remarks>IMPORTANT -- If the child class wants to implement its own OnEnable/Disable methods,
    /// they must be marked as <c>protected virtual</c> and call <c>base.OnEnable/Disable()</c> to maintain event subscription.</remarks>
    public abstract class SpecificMemoryListener : MemoryListener
    {
        [SerializeField, Tooltip("The memory key that will be checked for.")]
        protected Memory MemoryKey;

        protected override void onRemembered(Memory key)
        {
            if (key == MemoryKey)
                onRememberedKey();
        }

        protected abstract void onRememberedKey();

        protected override void onForgot(Memory key)
        {
            if (key == MemoryKey)
                onForgotKey();
        }
        
        protected abstract void onForgotKey();
    }
}
