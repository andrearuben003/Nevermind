using UnityEngine;

namespace Nevermind.Player
{
    public class PlayerAnimations : MonoBehaviour
    {

        private CharacterMovement movement;
        private CharacterJump jump;
        private MovementLimiter limiter;

        private Animator anim;

        void Start()
        {
            anim = GetComponent<Animator>();

            movement = GetComponent<CharacterMovement>();
            jump = GetComponent<CharacterJump>();

            limiter = GetComponent<MovementLimiter>();
        }


        private void Update()
        {
            anim.SetBool("IsRunning", movement.velocity.x != 0f);

            if (jump.currentlyJumping && jump.body.linearVelocityY > 0.01f)
                anim.SetTrigger("Jumping");

            if(jump.body.linearVelocityY < -0.01f)
                anim.SetTrigger("Falling");

            if (movement.onGround)
                anim.SetTrigger("OnGround");
        }
    }
}
