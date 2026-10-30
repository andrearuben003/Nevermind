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

        private void Update()
        {
            curTimer += Time.deltaTime;

            if (curTimer > timer)
            {
                GameObject.Instantiate(rocket, transform.position, Quaternion.identity);
                curTimer = 0f;
            }
        }
    }
}
