using Nevermind.Player.Controller;
using Neverming.Player;
using UnityEngine;


namespace Nevermind.Material 
{
    public class Slime : MonoBehaviour
    {
        [Tooltip("Indicates of much the player get pushed on exiting the slime collider")]
        [SerializeField] float pushOnExit = 20f;

        public float GetPushOnExit()
        {
            return pushOnExit;
        }


        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Change controller type to the omnidirectional one
            if (collision.CompareTag("Player"))
                PlayerStatsManager.Instance.EnterSlime();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // Go back to the base player controller type and give it a push when exiting
            if (collision.CompareTag("Player"))
                PlayerStatsManager.Instance.ExitSlime(pushOnExit);
        }
    }
}
