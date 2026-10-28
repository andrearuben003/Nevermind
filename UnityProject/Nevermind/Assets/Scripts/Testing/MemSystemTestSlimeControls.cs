using Nevermind.MemorySystem;
using UnityEngine;

namespace Nevermind.Testing
{
    public class MemSystemSlimeTestControls : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                MemoryManager.Instance.Remember(new Memory(MemoryKey.Slime));
            if (Input.GetKeyDown(KeyCode.W))
                MemoryManager.Instance.Forget(MemoryKey.Slime);
        }
    }
}
