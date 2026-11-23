using System;
using Nevermind.PlatformSurfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nevermind.Player.Controller {

    public class PlatformerController : MonoBehaviour, IIceAffected
    {
        [SerializeField] 
        private PlayerConfig config;
        
        #region Ice
        private bool _onIce;
        private float _inputX;
        private float _iceDirection;
        private float _stuckedSpeed = 0.05f;
        private bool _slideMomentum;
        private Rigidbody2D _rb;
        
        public bool IsOnIce => _onIce;
        public float SlideDir => _iceDirection;

        #endregion
        
        private CharacterMovement movement;
        private CharacterJump jump;
        private CharacterGround ground;
        private MovementLimiter movementLimiter;

        private void Awake()
        {
            movement = gameObject.AddComponent<CharacterMovement>();
            jump = gameObject.AddComponent<CharacterJump>();
            ground = gameObject.AddComponent<CharacterGround>();
            
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
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!movementLimiter.CanMove)
            {
                movement.directionX = 0f;
                return;
            }
            
            if(_slideMomentum && !movementLimiter.OnIce && _rb.linearVelocity.y <= 0.01f)
            {
                _slideMomentum = false;
                _iceDirection = 0f;
                CheckDirection();
            }
        }
        
        private void FixedUpdate()
        {
            
            //Check if player stuck on ice near a wall
            if(movementLimiter.OnIce && _iceDirection != 0f && Mathf.Abs(_rb.linearVelocity.x) < _stuckedSpeed)
            {
                _iceDirection = 0f;
                CheckDirection();
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
        #endregion

        public void CheckDirection()
        {
            if (!movementLimiter.CanMove)
            {
                movement.directionX = 0;
                return;
            }

            if (movementLimiter.OnIce)
            {//If sliding on ice keep direction
                if(_iceDirection == 0f && _inputX != 0f )
                    _iceDirection = Mathf.Sign(_inputX);
                
                movement.directionX = _iceDirection;
            }
            else if (_slideMomentum)
            {//Jumping from ice keep horizontal momentum
                movement.directionX = _iceDirection;
            }
            else
            {//Input movement
                movement.directionX = _inputX;
            }
        }
        
        #region Ice

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

            _slideMomentum = _rb.linearVelocity.y > 0.01f && _iceDirection != 0f;
            if(!_slideMomentum)
                _iceDirection = 0f;
            
            CheckDirection();
        }

        public void StartSliding(float dir)
        {
            _slideMomentum = false;
            _iceDirection = dir;
            CheckDirection();
        }

        public void StopSliding()
        {
            _slideMomentum = false;
            _iceDirection = 0f;
            movement.directionX = 0f;
            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.TryGetComponent<IIceAffected>(out IIceAffected otherIce))
                IceCollision.TransferMomentum(this, other);
        }

        #endregion
    }
}
