using Nevermind.PlatformSurfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nevermind.Player.Controller {

    public class PlatformerController : MonoBehaviour, IIceAffected
    {
        CharacterMovement movement;
        CharacterJump jump;
        CharacterGround ground;

        [SerializeField] private PlayerConfig config;

        Rigidbody2D rb;
        MovementLimiter movementLimiter;

        private bool _onIce;
        private float _inputX;
        private float _iceDirection;
        private bool _slideMomentum;

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
            
            _onIce = false;
        }

        private void Update()
        {
            if (!movementLimiter.CanMove)
            {
                movement.directionX = 0f;
                return;
            }
            
            if(_slideMomentum && !movementLimiter.OnIce && rb.linearVelocity.y <= 0.01f)
            {
                _slideMomentum = false;
                _iceDirection = 0f;
                CheckDirection();
            }
        }

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
                _inputX = context.ReadValue<float>();
                CheckDirection();
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

        public void Bounce(Vector2 dir, float bounceAmount)
        {
            rb.AddForce(dir * bounceAmount, ForceMode2D.Impulse);
        }

        public void CheckDirection()
        {
            if (!movementLimiter.CanMove)
            {
                movement.directionX = 0;
                return;
            }

            if (movementLimiter.OnIce)
            {
                if(_iceDirection == 0f && _inputX != 0f )
                    _iceDirection = Mathf.Sign(_inputX);
                
                movement.directionX = _iceDirection;
            }
            else if (_slideMomentum)
            {
                movement.directionX = _iceDirection;
            }
            else
            {
                movement.directionX = _inputX;
            }
        }

        public void EnterIce()
        {
            movementLimiter.OnIce = true;
            if(!_slideMomentum)
                _iceDirection = 0f;
            
            _slideMomentum = false;
            
            CheckDirection();
        }

        public void ExitIce()
        {
            movementLimiter.OnIce = false;

            _slideMomentum = rb.linearVelocity.y > 0.01f && _iceDirection != 0f;
            if(!_slideMomentum)
                _iceDirection = 0f;
            
            CheckDirection();
        }
    }
}
