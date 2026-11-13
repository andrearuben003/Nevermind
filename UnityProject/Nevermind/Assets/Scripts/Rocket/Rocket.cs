using UnityEngine;

namespace Nevermind.Rocket
{
    [SelectionBase, RequireComponent(typeof(Rigidbody2D))]
    public class Rocket : MonoBehaviour
    {

        [SerializeField]
        private float movementSpeed;
        [SerializeField]
        private float slowedSpeed;

        private float currentSpeed;

        [SerializeField]
        private Vector2 direction;

        [SerializeField]
        private float rayLength;
        [SerializeField]
        private LayerMask collisionMasks;
        [SerializeField]
        private LayerMask destroyableMasks;

        private Rigidbody2D rb;

        bool isDangerous;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            currentSpeed = movementSpeed;

            isDangerous = true;
        }

        private void Update()
        {
            if (isDangerous)
            {
                if (Physics2D.Raycast(transform.position, direction, rayLength, collisionMasks))
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, destroyableMasks);
                    if (hit)
                        Destroy(hit.collider.gameObject);

                    Explode();
                }
            }

            transform.position = transform.position + transform.right * (currentSpeed * Time.deltaTime);
        }

        private void Explode()
        {
            //Destroy itself
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right * rayLength);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Slime"))
            {
                currentSpeed = slowedSpeed;

                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.mass = 1f;
                rb.gravityScale = 0f;
                rb.linearDamping = 5f;
                rb.freezeRotation = true;

                isDangerous = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Slime"))
            {
                currentSpeed = movementSpeed;

                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;

                isDangerous = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
                collision.transform.parent = transform;
            else if(collision.collider.GetComponent<Rocket>())
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
                if(gameObject.activeSelf) //to fix a bug when closing the game
                    collision.transform.parent = null;
        }
    }
}