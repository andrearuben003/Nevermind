using UnityEngine;

namespace Nevermind.Interactables
{
    [SelectionBase]
    [RequireComponent(typeof(TargetJoint2D))]
    public class Grabbable : Interactable
    {
        private static Transform playerTransform;

        [SerializeField]
        private Vector2 carryOffset;
        [SerializeField, Min(0)]
        private float throwForce = 2f;
        
        private Rigidbody2D rb;
        private TargetJoint2D joint;
        private Collider2D coll;
        private bool isGrabbed;
        public bool IsGrabbed
        {
            get => isGrabbed;
            private set  
            {
                isGrabbed = value;
                
                transform.position = playerTransform.TransformPoint(carryOffset);
            
                joint.enabled = isGrabbed;
            
                coll.isTrigger = isGrabbed;

                overlapCount = 0;
            }
        }

        private TagHandle playerTag;
        private int overlapCount;

        private void Awake()
        {
            if (!playerTransform)
                playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            rb = GetComponent<Rigidbody2D>();
            joint = GetComponent<TargetJoint2D>();
            coll =  GetComponent<Collider2D>();
            
            playerTag = TagHandle.GetExistingTag("Player");
        }

        private void FixedUpdate()
        {
            if (!IsGrabbed) return;
            
            Vector2 targ = playerTransform.TransformPoint(carryOffset);
            joint.target = targ;
        }

        public override void Interact()
        {
            if (overlapCount > 0) return;
            
            IsGrabbed = !IsGrabbed;
        }

        public override void InteractHoldStart()
        {
            if (!isGrabbed)
            {
                IsGrabbed = true;
                return;
            }
            
            if (overlapCount > 0) return;
            
            IsGrabbed = false;
            rb.linearVelocity = Vector2.right * (playerTransform.localScale.x * throwForce);
        }
        public override void InteractHoldEnd() {}

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(playerTag) || other.isTrigger) return;
            
            overlapCount++;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(playerTag) || other.isTrigger) return;
            
            overlapCount--;
        }
    }
}