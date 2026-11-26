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

            movement = GetComponentInParent<CharacterMovement>();
            jump = GetComponentInParent<CharacterJump>();

            limiter = GetComponentInParent<MovementLimiter>();
        }


        private void Update()
        {
            anim.SetFloat("xMovement", Mathf.Abs(movement.velocity.x));
            anim.SetFloat("yMovement", jump.velocity.y);
            anim.SetBool("onGround", jump.onGround);
        }
    }
}
