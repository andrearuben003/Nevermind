using UnityEngine;

namespace Nevermind.MemorySystem
{
    /// Base class for MonoBehaviours that want to listen for memory events.
    /// <remarks>IMPORTANT -- If the child class wants to implement its own OnEnable/Disable methods,
    /// they must be marked as <c>protected virtual</c> and call <c>base.OnEnable/Disable()</c> to maintain event subscription.</remarks>
    public abstract class MemoryListener : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            MemoryManager.Instance.RememberedMemory += onRemembered;
            MemoryManager.Instance.ForgotMemory += onForgot;
        }

        protected virtual void OnDisable()
        {
            MemoryManager.Instance.RememberedMemory -= onRemembered;
            MemoryManager.Instance.ForgotMemory -= onForgot;
        }

        protected abstract void onRemembered(Memory key);

        protected abstract void onForgot(Memory key);
    }
}
