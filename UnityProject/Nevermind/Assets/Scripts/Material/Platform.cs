using Neverming.Player;
using UnityEngine;

namespace Nevermind.Material
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Platform : MonoBehaviour
    {
        Rigidbody2D rb;
        BoxCollider2D col;

        [SerializeField]
        LayerMask slimeMask;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;

            col = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {

            if(Physics2D.OverlapBox(transform.position + (Vector3)col.offset, col.size, 0f, slimeMask))
            {
                //Platforms can be moved when they are inside a slime
                rb.bodyType = RigidbodyType2D.Dynamic;
            } 
            else
            {
                //On default, platforms are fixed element in the environment
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }
}

