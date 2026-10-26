using UnityEngine;

namespace Platform_Movements
{

    public enum RotationDirection
    {
        Clockwise = -1,
        CounterClockwise = 1
    }
    
    public class PlatformRotation : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private RotationDirection rotationDirection = RotationDirection.Clockwise;

        private Rigidbody2D _rb;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
            _rb.MoveRotation(_rb.rotation + (int)rotationDirection * rotationSpeed * Time.fixedDeltaTime);

        }
    }
}
