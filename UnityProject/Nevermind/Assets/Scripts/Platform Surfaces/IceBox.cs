using System;
using UnityEngine;

namespace Nevermind.PlatformSurfaces
{
    public class IceBox : MonoBehaviour, IIceAffected
    {
        
        private bool _onIce;
        private Rigidbody2D _rb;
        private float _slideDir;
        
        public bool IsOnIce => _onIce;
        public float SlideDir => _slideDir;
        
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
            
            float velX = _rb.linearVelocity.x;

            if (_slideDir == 0f)
            {
                if (Mathf.Abs(velX) <= toleranceSpeed)
                    return;
                
                _slideDir = Mathf.Sign(velX);
            }
            else if (Mathf.Abs(velX) <= toleranceSpeed || Mathf.Sign(velX) != _slideDir)
            {
                _slideDir = 0f;
                return;
            }
            
            _rb.linearVelocity = new Vector2(_slideDir * speed, _rb.linearVelocity.y);

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

        public void StartSliding(float dir)
        {
            _slideDir = dir;
            _rb.linearVelocity = new Vector2(dir * speed, _rb.linearVelocity.y);
        }

        public void StopSliding()
        {
            _slideDir = 0f;
            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.TryGetComponent<IIceAffected>(out IIceAffected otherIce))
                IceCollision.TransferMomentum(this, other);
        }
        
    }
}
