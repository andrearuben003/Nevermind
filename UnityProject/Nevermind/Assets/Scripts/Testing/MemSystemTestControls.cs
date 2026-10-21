using Nevermind.MemorySystem;
using UnityEngine;

namespace Nevermind.Testing
{
    public class MemSystemTestControls : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                MemoryManager.Instance.Remember(new Memory(MemoryKey.Test, MemoryType.SemiPermanent));
            if (Input.GetKeyDown(KeyCode.W))
                MemoryManager.Instance.Forget(MemoryKey.Test);
        }
    }
}
