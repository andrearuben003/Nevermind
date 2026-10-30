using Nevermind.Material;
using System.Collections;
using System.Runtime.InteropServices;
using TarodevController;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Nevermind.Player.Controller
{
    /// This class gives the possibility of having multiple controller type on the same player
    [RequireComponent(typeof(PlatformerController), typeof(OmnidirectionalController))]
    public class PlayerControllersManager : MonoBehaviour
    {
        #region  Controller
        //Base controller
        public PlatformerController PlatformerController { get; private set; }

        //Omnidirectional controller
        public OmnidirectionalController OmnidirectionalController { get; private set; }

        private PlayerInput input;

        private ControllerType type;
        #endregion

        //External reference to the class
        public static PlayerControllersManager Instace {  get; private set; }
        private void Awake()
        {
            Instace = this;

            PlatformerController = GetComponent<PlatformerController>();
            OmnidirectionalController = GetComponent<OmnidirectionalController>();

            input = GetComponent<PlayerInput>();

            ChangeControllerType(ControllerType.Default);
        }

        /// This function enable the specified player controller and disable all the others
        public void ChangeControllerType(ControllerType type)
        {
            this.type = type;
            switch (type)
            {
                case ControllerType.Default:
                    PlatformerController.enabled = true;
                    OmnidirectionalController.enabled = false;
                    input.SwitchCurrentActionMap("BaseController");
                    break;
                case ControllerType.Omnidirectional:
                    PlatformerController.enabled = false;
                    OmnidirectionalController.enabled = true;
                    input.SwitchCurrentActionMap("OmnidirectionalController");
                    break;
            }
        }

        /*
        /// Change controller type to the omnidirectional one
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Slime"))
            {
                ChangeControllerType(ControllerType.Omnidirectional);
            }
        }

        /// Go back to the base player controller type and give it a push when exiting
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Slime"))
            {
                Vector2 inputDir = omnidirectionalController.GetInputDir();
                ChangeControllerType(ControllerType.Default);
                platformerController.Bounce(inputDir, collision.GetComponent<Slime>().GetPushOnExit());
            }
        }*/

    }

    public enum ControllerType
    {
        Default,
        Omnidirectional
    }
}
