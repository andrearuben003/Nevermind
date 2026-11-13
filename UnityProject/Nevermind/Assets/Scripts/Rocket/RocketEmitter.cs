using UnityEngine;

namespace Nevermind.Rocket
{
    [SelectionBase]
    public class RocketEmitter : MonoBehaviour
    {
        [SerializeField]
        private GameObject rocketPrefab;
        [SerializeField]
        private Transform shotOrigin;
        [SerializeField, Min(0.1f)]
        private float shotInterval;
        
        private float timer;

        private void Awake()
        {
            timer = shotInterval;
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer > shotInterval)
            {
                Instantiate(rocketPrefab, shotOrigin.position, shotOrigin.rotation);
                timer = 0f;
            }
        }
    }
}
