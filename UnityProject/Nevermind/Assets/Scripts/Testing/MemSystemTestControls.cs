using Nevermind.MemorySystem;
using UnityEngine;

namespace Nevermind.Testing
{
    public class MemSystemTestControls : MonoBehaviour
    {
        [SerializeField]
        private Memory controlledMemory;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                MemoryManager.Instance.Remember(controlledMemory);
            if (Input.GetKeyDown(KeyCode.W))
                MemoryManager.Instance.Forget(controlledMemory.Key);
        }
    }
}
