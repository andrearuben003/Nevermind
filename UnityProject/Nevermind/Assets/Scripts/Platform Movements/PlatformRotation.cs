using UnityEngine;

namespace Nevermind.PlatformMovements
{
    public enum RotationDirection
    {
        Clockwise = -1,
        CounterClockwise = 1
    }
    
    public class PlatformRotation : MonoBehaviour
    {
        [SerializeField, Min(0)] 
        private float rotationSpeed = 1f;
        [SerializeField] 
        private RotationDirection rotationDirection;

        private Rigidbody2D _rb;

        void Awake()
        {
            //_rb = GetComponent<Rigidbody2D>();
        }
        
        void FixedUpdate()
        {
            transform.Rotate(Vector3.forward, (int)rotationDirection * rotationSpeed * Time.fixedDeltaTime);
            //_rb.MoveRotation(_rb.rotation + (int)rotationDirection * rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
