using UnityEngine;
using UnityEngine.InputSystem;

namespace Nevermind.Player.Controller
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerControllersManager : MonoBehaviour
    {
        public static PlayerControllersManager Instance {  get; private set; }

        [SerializeField]
        private GameObject player;
        public PlatformerController PlatformerController { get; private set; }
        public OmnidirectionalController OmnidirectionalController { get; private set; }

        private PlayerInput input;
        
        private void Awake()
        {
            if (Instance)
                Destroy(gameObject);
            
            Instance = this;

            input = GetComponent<PlayerInput>();
            PlatformerController = player.GetComponent<PlatformerController>();
            OmnidirectionalController = player.GetComponent<OmnidirectionalController>();
            
            ChangeControllerType(ControllerType.Default);
        }

        /// This function enable the specified player controller and disable all the others
        public void ChangeControllerType(ControllerType type)
        {
            switch (type)
            {
                case ControllerType.Default:
                    PlatformerController.enabled = true;
                    OmnidirectionalController.enabled = false;
                    input.SwitchCurrentActionMap("Base Controller");
                    break;
                case ControllerType.Omnidirectional:
                    PlatformerController.enabled = false;
                    OmnidirectionalController.enabled = true;
                    input.SwitchCurrentActionMap("Omnidirectional Controller");
                    break;
            }
        }
    }

    public enum ControllerType
    {
        Default,
        Omnidirectional
    }
}
