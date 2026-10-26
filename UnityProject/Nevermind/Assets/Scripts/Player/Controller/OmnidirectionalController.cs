using UnityEngine;
using UnityEngine.InputSystem;


namespace Nevermind.Player.Controller
{

    /// This class is meant to be applied to the player when you want to be able to move freely in all directions (using WASD)
    public class OmnidirectionalController : MonoBehaviour
    {
        [SerializeField] float speed;

        private Rigidbody2D rb;

        private Vector2 inputDir;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
        }

        protected virtual void OnEnable()
        {
            rb.gravityScale = 0f;
        }

        private void Update()
        {
            rb.linearVelocity = inputDir * speed;
        }

        public Vector2 GetInputDir()
        {
            return inputDir;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            inputDir = context.ReadValue<Vector2>();
        }

    }
}
