using UnityEngine;

namespace Nevermind.MemorySystem
{
    /// Base class for MonoBehaviours that want to listen for memory events.
    /// <remarks>IMPORTANT -- If the child class wants to implement its own OnEnable/Disable methods,
    /// they must be marked as <c>protected virtual</c> and call <c>base.OnEnable/Disable()</c> to maintain event subscription.</remarks>
    public abstract class MemoryListener : MonoBehaviour
    {
        [SerializeField, Tooltip("The memory key that will be checked for.")]
        protected MemoryKey MemoryKey;
        
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

        protected abstract void onRemembered(MemoryKey key);

        protected abstract void onForgot(MemoryKey key);
    }
}
