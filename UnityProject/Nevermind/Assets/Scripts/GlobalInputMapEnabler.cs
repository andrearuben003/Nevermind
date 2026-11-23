using UnityEngine;
using UnityEngine.InputSystem;

namespace Nevermind
{
    [RequireComponent(typeof(PlayerInput))]
    public class GlobalInputMapEnabler : MonoBehaviour
    {
        private PlayerInput playerInput;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            playerInput.actions.FindActionMap("Global").Enable();
        }
    }
}
