using UnityEngine;

namespace Nevermind.LevelSystem
{
    [SelectionBase, RequireComponent(typeof(Collider2D))]
    public class Checkpoint : MonoBehaviour
    {
        private TagHandle playerTag;

        private void Awake()
        {
            playerTag = TagHandle.GetExistingTag("Player");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;
            
            Debug.Log("Checkpoint reached");
            
            LevelManager.Instance.RecordPlayer(transform.position);
        }
    }
}
