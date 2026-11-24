using UnityEngine;

namespace Nevermind.LevelSystem
{
    [SelectionBase, RequireComponent(typeof(Collider2D))]
    public class KillZone : MonoBehaviour
    {
        private TagHandle playerTag;

        private void Awake()
        {
            playerTag = TagHandle.GetExistingTag("Player");
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(playerTag))
                RespawnManager.Instance.Reload();
            else if (other.gameObject.TryGetComponent(out Killable killable))
                killable.Kill();
        }
    }
}
