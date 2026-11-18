using System;
using Nevermind.Player.Controller;
using UnityEngine;

namespace Nevermind.Material 
{
    public class Slime : MonoBehaviour
    {
        [Tooltip("Indicates of much the player get pushed on exiting the slime collider")]
        [SerializeField] private float pushOnExit = 20f;

        private TagHandle playerTag;

        private void Awake()
        {
            playerTag = TagHandle.GetExistingTag("Player");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(playerTag))
                PlayerControllersManager.Instance.ChangeControllerType(ControllerType.Omnidirectional);
            
            //TODO slime interaction with non player objects
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(playerTag))
            {
                PlayerControllersManager.Instance.ChangeControllerType(ControllerType.Default);
                Vector2 exitDir = PlayerControllersManager.Instance.OmnidirectionalController.GetInputDir();
                other.attachedRigidbody.AddForce(exitDir * pushOnExit, ForceMode2D.Impulse);
            }
            
            //TODO slime interaction with non player objects
        }
    }
}
