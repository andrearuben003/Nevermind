using UnityEngine;

namespace Nevermind.Rocket
{
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

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            currentSpeed = movementSpeed;
        }

        private void Update()
        {
            if (Physics2D.Raycast(transform.position, direction, rayLength, collisionMasks))
            {
                Explode();
            }

            transform.position = transform.position + (Vector3)direction * currentSpeed * Time.deltaTime;
        }

        private void Explode()
        {
            //Destroy itself
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * rayLength);
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
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Slime"))
            {
                currentSpeed = movementSpeed;

                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
                collision.transform.parent = transform;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
                if(gameObject.activeSelf) //to fix a bug when closing the game
                collision.transform.parent = null;
        }
    }
}