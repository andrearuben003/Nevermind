using UnityEngine;

namespace Nevermind.Interactables
{
    public class Turner : Interactable
    {
        private static Transform playerTransform;
        
        [SerializeField]
        private Transform target;
        [SerializeField]
        private float rotationSpeed;

        private bool turning;
        private void Awake()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void FixedUpdate()
        {
            if (!turning) return;
            
            int dir = playerTransform.position.x > transform.position.x ? 1 : -1;

            float turn = dir * rotationSpeed * Time.deltaTime;
            transform.Rotate(transform.forward, turn);
            target.Rotate(target.forward, turn);
        }

        public override void InteractHoldStart()
        {
            turning = true;
        }

        public override void InteractHoldEnd()
        {
            turning = false;
        }

        public override void Deselect()
        {
            base.Deselect();
            turning = false;
        }
    }
}
