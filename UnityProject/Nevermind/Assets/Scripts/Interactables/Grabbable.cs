using UnityEngine;

namespace Nevermind.Interactables
{
    [SelectionBase]
    [RequireComponent(typeof(TargetJoint2D))]
    public class Grabbable : Interactable
    {
        private static Transform playerTransform;
        private static Rigidbody2D playerRigidbody2D;
        // If mass is set too low the object starts clipping through walls
        private const float WeightlessMass = 0.1f;
        // If the object gets too far while grabbed, something might be in the way so become a trigger to pass through
        private const float GhostDistanceIn = 1f;
        private const float GhostDistanceOut = 0.2f;

        [SerializeField]
        private Vector2 carryOffset;
        [SerializeField, Tooltip("I")]
        private bool weightlessCarry = true;
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
            
                if (weightlessCarry)
                    rb.mass = isGrabbed ? WeightlessMass : startMass;
           
                if (!isGrabbed && playerRigidbody2D.linearVelocity.sqrMagnitude > 1f)
                    rb.AddForce(playerRigidbody2D.linearVelocity.normalized * throwForce, ForceMode2D.Impulse);
                
                if (!isGrabbed)
                    coll.isTrigger = false;
            }
        }

        private float startMass;

        private void Awake()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            playerRigidbody2D = playerTransform.GetComponent<Rigidbody2D>();

            rb = GetComponent<Rigidbody2D>();
            joint = GetComponent<TargetJoint2D>();
            coll =  GetComponent<Collider2D>();
            
            startMass = rb.mass;
        }

        private void FixedUpdate()
        {
            if (!IsGrabbed) return;
            
            Vector2 targ = playerTransform.TransformPoint(carryOffset);
            joint.target = targ;

            if (Vector3.Distance(transform.position, targ) > GhostDistanceIn)
                coll.isTrigger = true;
            else if (Vector3.Distance(transform.position, targ) < GhostDistanceOut)
                coll.isTrigger = false;
        }

        public override void Interact()
        {
            IsGrabbed = !IsGrabbed;
        }
    }
}