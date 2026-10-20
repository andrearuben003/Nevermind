using Nevermind.MemorySystem;
using UnityEngine;

namespace Nevermind.Testing
{
    public class MemSystemTestControls : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                MemoryManager.Instance.Remember(new Memory(MemoryKey.Test));
            if (Input.GetKeyDown(KeyCode.W))
                MemoryManager.Instance.Forget(new Memory(MemoryKey.Test));
        }
    }
}
