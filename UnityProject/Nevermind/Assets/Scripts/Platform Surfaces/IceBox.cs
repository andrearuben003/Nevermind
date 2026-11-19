using UnityEngine;

namespace Nevermind.PlatformSurfaces
{
    public class IceBox : MonoBehaviour, IIceAffected
    {
        
        private bool _onIce = false;
        private Rigidbody2D _rb;
        
        [SerializeField] private float speed = 10.0f;
        [SerializeField] private float toleranceSpeed = 0.01f;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _onIce = false;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
            if (!_onIce)
                return;

            Debug.Log("IceBox");
            if(Mathf.Abs(_rb.linearVelocity.x) > toleranceSpeed)
                _rb.linearVelocity = new Vector2(Mathf.Sign(_rb.linearVelocity.x) * speed, _rb.linearVelocity.y);

        }

        public void EnterIce()
        {
            _onIce = true;
        }

        public void ExitIce()
        {
            _onIce = false;
            
            if(Mathf.Abs(_rb.linearVelocity.y) <= 0.01f)
                _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
        }
    }
}
