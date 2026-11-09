using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nevermind.Player.Controller {

    public class PlatformerController : MonoBehaviour
    {
        #region General
        CharacterMovement movement;
        CharacterJump jump;
        CharacterGround ground;

        [SerializeField] private PlayerConfig config;

        Rigidbody2D rb;
        MovementLimiter movementLimiter;
        #endregion

        #region Grab
        Transform grabbedObject;

        #endregion

        private void Awake()
        {
            movement = gameObject.AddComponent<CharacterMovement>();
            jump = gameObject.AddComponent<CharacterJump>();
            ground = gameObject.AddComponent<CharacterGround>();

            rb = GetComponent<Rigidbody2D>();

            movementLimiter = gameObject.AddComponent<MovementLimiter>();

            //Setting up
            movement.maxSpeed = config.maxSpeed;
            movement.maxAcceleration = config.maxAcceleration;
            movement.maxDecceleration = config.maxDecceleration;
            movement.maxTurnSpeed = config.maxTurnSpeed;
            movement.maxAirAcceleration = config.maxAirAcceleration;
            movement.maxAirDeceleration = config.maxAirDeceleration;
            movement.maxAirTurnSpeed = config.maxAirTurnSpeed;
            movement.friction = config.friction;
            movement.useAcceleration = config.useAcceleration;

            jump.jumpHeight = config.jumpHeight;
            jump.timeToJumpApex = config.timeToJumpApex;
            jump.upwardMovementMultiplier = config.upwardMovementMultiplier;
            jump.downwardMovementMultiplier = config.downwardMovementMultiplier;
            jump.maxAirJumps = config.maxAirJumps;
            jump.variablejumpHeight = config.variablejumpHeight;
            jump.jumpCutOff = config.jumpCutOff;
            jump.speedLimit = config.speedLimit;
            jump.coyoteTime = config.coyoteTime;
            jump.jumpBuffer = config.jumpBuffer;

            ground.groundLength = config.groundLength;
            ground.colliderOffset = config.colliderOffset;
            ground.groundLayer = config.groundLayer;
        }

        private void Update()
        {
            if (!movementLimiter.CanMove)
            {
                movement.directionX = 0f;
            }

            if (grabbedObject)
            {
                jump.jumpHeight = config.jumpHeight + config.grabJumpHeightAddition * grabbedObject.transform.parent.childCount;

                movement.maxSpeed = config.maxSpeed + config.grabMaxSpeedAddition * grabbedObject.transform.parent.childCount;
                movement.maxAcceleration = config.maxAcceleration + config.grabMaxAcceleration * grabbedObject.transform.parent.childCount;

                //When something push the box away from the player
                if(Vector2.Distance(transform.position, grabbedObject.position) > config.grabRange)
                {
                    ReleaseBox();
                }
            }
        }

        #region General
        protected virtual void OnEnable()
        {
            movement.enabled = true;
            jump.enabled = true;
            ground.enabled = true;
        }

        protected virtual void OnDisable()
        {
            movement.enabled = false;
            jump.enabled = false;
            ground.enabled = false;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            //This is called when you input a direction on a valid input type, such as arrow keys or analogue stick
            //The value will read -1 when pressing left, 0 when idle, and 1 when pressing right.
            if (movementLimiter.CanMove)
            {
                movement.directionX = context.ReadValue<float>();
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            //This function is called when one of the jump buttons (like space or the A button) is pressed.
            //When we press the jump button, tell the script that we desire a jump.
            //Also, use the started and canceled contexts to know if we're currently holding the button
            if (movementLimiter.CanJump)
            {
                if (context.started)
                {
                    jump.desiredJump = true;
                    jump.pressingJump = true;
                }

                if (context.canceled)
                {
                    jump.pressingJump = false;
                }
            }
        }
        #endregion

        #region Grab

        public void OnGrab(InputAction.CallbackContext context)
        {
            if (context.started){
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right * transform.localScale.x, config.grabRange, config.grabLayer);
                if (hit)
                {
                    grabbedObject = hit.transform;

                    Box box = grabbedObject.GetComponent<Box>();

                    if (box)
                    {
                        Transform group = box.transform.parent;

                        if (!group) //Player touch a box without a group
                        {
                            //Create new parent group
                            group = (new GameObject("Box Group")).transform;
                            group.gameObject.AddComponent<Rigidbody2D>();
                            group.GetComponent<Rigidbody2D>().freezeRotation = true;

                            box.transform.parent = group;
                            box.GetJoint().enabled = true;
                            box.GetJoint().connectedBody = group.GetComponent<Rigidbody2D>();


                            //Connect the group to the player
                            group.AddComponent<FixedJoint2D>().connectedBody = rb;
                        } 
                        else
                        {
                            FixedJoint2D joint = group.GetComponent<FixedJoint2D>();

                            if (joint)
                            {
                                joint.enabled = true;
                                joint.connectedBody = rb;
                            } 
                            else
                            {
                                group.AddComponent<FixedJoint2D>().connectedBody = rb;
                            }
                        }

                        box.FreeBoxGroupPhysics();
                        box.ResetBoxGroupGravityScale();

                        group.transform.parent = transform;
                    }
                }
            }
            else if (context.canceled){
                if (grabbedObject)
                {
                    Box tmp = grabbedObject.GetComponent<Box>();
                    ReleaseBox();

                    //Throw
                    if(rb.linearVelocity != Vector2.zero)
                    {
                        Vector2 dir = new Vector2(config.throwDirection.x * transform.localScale.x, config.throwDirection.y);
                        tmp.Throw(dir , config.throwForce);
                    }
                }
            }
        }

        public void ReleaseBox()
        {
            Box box = grabbedObject.GetComponent<Box>();

            if (box)
            {
                FixedJoint2D joint = box.transform.parent.GetComponent<FixedJoint2D>();

                if (joint)
                {
                    joint.connectedBody = null;
                    joint.enabled = false;
                }
            }

            //Reset original values
            jump.jumpHeight = config.jumpHeight;

            movement.maxSpeed = config.maxSpeed;
            movement.maxAcceleration = config.maxAcceleration;

            grabbedObject.transform.parent.parent = null;

            grabbedObject = null;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * config.grabRange);
        }

        #endregion

        public void Bounce(Vector2 dir, float bounceAmount)
        {
            rb.AddForce(dir * bounceAmount, ForceMode2D.Impulse);
        }
    }
}
