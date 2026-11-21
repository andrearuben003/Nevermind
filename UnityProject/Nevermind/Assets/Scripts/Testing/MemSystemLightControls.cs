using Nevermind.MemorySystem;
using UnityEngine;

namespace Nevermind.Testing
{
    public class MemSystemLightControls : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                MemoryManager.Instance.Remember(Memory.Light);
            if (Input.GetKeyDown(KeyCode.W))
                MemoryManager.Instance.Forget(Memory.Light);
        }
    }
}
