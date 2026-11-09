using UnityEngine;

namespace Nevermind.Rocket
{
    public class RocketEmitter : MonoBehaviour
    {
        
        //TODO: maybe is better to use pooling


        [SerializeField]
        private GameObject rocket;

        [SerializeField]
        private float timer;
        private float curTimer;

        private void Awake()
        {
            curTimer = timer;
        }

        private void Update()
        {
            curTimer += Time.deltaTime;

            if (curTimer > timer)
            {
                Rocket tmp = GameObject.Instantiate(rocket, transform.position, Quaternion.identity).GetComponent<Rocket>();
                tmp.SetDirection(transform.right);
                curTimer = 0f;
            }
        }
    }
}
