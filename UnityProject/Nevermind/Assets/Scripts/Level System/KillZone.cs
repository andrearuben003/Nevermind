using UnityEngine;

namespace Nevermind.LevelSystem
{
    [SelectionBase, RequireComponent(typeof(Collider2D))]
    public class KillZone : MonoBehaviour
    {
        private TagHandle playerTag;
        private TagHandle bubbleTag;

        private void Awake()
        {
            playerTag = TagHandle.GetExistingTag("Player");
            bubbleTag = TagHandle.GetExistingTag("Thought Bubble");
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(playerTag))
                LevelManager.Instance.Reload();
            else if (other.gameObject.CompareTag(bubbleTag))
                Destroy(other.gameObject);
        }
    }
}
