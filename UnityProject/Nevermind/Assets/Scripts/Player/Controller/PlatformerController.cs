using UnityEngine;
using UnityEngine.InputSystem;

namespace Nevermind.Player.Controller {

    public class PlatformerController : MonoBehaviour
    {
        CharacterMovement movement;
        CharacterJump jump;
        CharacterGround ground;

        [SerializeField] private PlayerConfig config;

        Rigidbody2D rb;

        private void Awake()
        {
            movement = gameObject.AddComponent<CharacterMovement>();
            jump = gameObject.AddComponent<CharacterJump>();
            ground = gameObject.AddComponent<CharacterGround>();

            rb = GetComponent<Rigidbody2D>();

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
            movement.directionX = context.ReadValue<float>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            //This function is called when one of the jump buttons (like space or the A button) is pressed.
            //When we press the jump button, tell the script that we desire a jump.
            //Also, use the started and canceled contexts to know if we're currently holding the button
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

        public void Bounce(Vector2 dir, float bounceAmount)
        {
            rb.AddForce(dir * bounceAmount, ForceMode2D.Impulse);
        }
    }
}
