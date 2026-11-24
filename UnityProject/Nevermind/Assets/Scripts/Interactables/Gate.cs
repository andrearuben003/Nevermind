using System.Collections;
using UnityEngine;

namespace Nevermind.Interactables
{
    [SelectionBase]
    public class Gate : MonoBehaviour
    {
        [SerializeField, Min(0)]
        private float animationTime = 0.2f;
        [SerializeField]
        private Transform gatePivot;
        [SerializeField]
        private Collider2D gateCollider;

        private void Start()
        {
            CloseGate();
        }

        public void OpenGate()
        {
            Debug.Log("Gate OPENED");
            
            gateCollider.enabled = false;
            
            StopAllCoroutines();
            StartCoroutine(animateGate(0));
        }

        public void CloseGate()
        {
            Debug.Log("Gate CLOSED");
            
            gateCollider.enabled = true;
            
            StopAllCoroutines();
            StartCoroutine(animateGate(1));
        }

        private IEnumerator animateGate(float finalScale)
        {
            Vector3 target = new(1, finalScale, 1);
            
            while (Mathf.Abs(gatePivot.localScale.y - finalScale) > 0.01f)
            {
                gatePivot.localScale = Vector3.Slerp(gatePivot.localScale, target, Time.deltaTime / animationTime);
                yield return null;
            }
            
            gatePivot.localScale = target;
        }
    }
}
