using UnityEditor;
using UnityEngine;

namespace Nevermind.LevelSystem
{
    [SelectionBase]
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        protected GameObject Prefab;

        private void Awake()
        {
            spawn();
        }

        protected virtual void spawn()
        {
            GameObject gO = Instantiate(Prefab, transform.position, Quaternion.identity);
            if (gO.TryGetComponent(out Killable killable))
            {
                killable.Killed += spawn;
            }
        }
        
        private void OnDrawGizmos()
        {
            GUIStyle style = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                normal =
                {
                    textColor = Color.royalBlue,
                }
            };
            Handles.Label(transform.position + Vector3.down * 0.5f, new GUIContent(Prefab ? Prefab.name : "EMPTY"), style);
        }
    }
}