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
        private TagHandle boxTag;

        private void Awake()
        {
            playerTag = TagHandle.GetExistingTag("Player");
            boxTag = TagHandle.GetExistingTag("Box");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(playerTag))
                PlayerControllersManager.Instance.ChangeControllerType(ControllerType.Omnidirectional);

            if (other.CompareTag(boxTag))
            {
                other.attachedRigidbody.gravityScale = 0f;
                other.attachedRigidbody.linearVelocityY = 0f;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(playerTag))
            {
                PlayerControllersManager.Instance.ChangeControllerType(ControllerType.Default);
                Vector2 exitDir = PlayerControllersManager.Instance.OmnidirectionalController.GetInputDir();
                other.attachedRigidbody.AddForce(exitDir * pushOnExit, ForceMode2D.Impulse);
            }

            if (other.CompareTag(boxTag))
                other.attachedRigidbody.gravityScale = 1f;
        }
    }
}
