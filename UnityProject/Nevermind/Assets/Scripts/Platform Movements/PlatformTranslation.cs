using System.Collections.Generic;
using UnityEngine;

namespace Platform_Movements
{
    public class PlatformTranslation : MonoBehaviour
    {
        [SerializeField] private List<Transform> pathTargets;
        [SerializeField] private float speed = 1f;

        [Tooltip("Tolerance value to consider the entity on top of the platform")]
        [SerializeField] private float topTolerance = 0.1f;

        private Rigidbody2D _rb;
        private Collider2D _collider;
        private int _currentTargetIndex = 0;

        //Entities on the platform
        private HashSet<Rigidbody2D> _entities;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            
            _entities = new HashSet<Rigidbody2D>();

            _currentTargetIndex++;
            _currentTargetIndex %= pathTargets.Count;
        }

        private void FixedUpdate()
        {
            Vector2 current = _rb.position;
            Vector2 target = pathTargets[_currentTargetIndex].position;
            Vector2 next = Vector2.MoveTowards(current, target, speed * Time.fixedDeltaTime);
            Vector2 delta = next - current;

            _rb.MovePosition(next);

            if (delta != Vector2.zero)
            {//Moves entities on the platform
                foreach (Rigidbody2D e in _entities)
                    e.position += delta;
            }

            if(Vector2.Distance(next, target) <= 0.0001f){
                _currentTargetIndex++;
                _currentTargetIndex %= pathTargets.Count;
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player") || other.rigidbody == null) 
                return;

            // Check if entity is on top of the platform
            if (other.collider.bounds.min.y >= _collider.bounds.max.y - topTolerance) 
                _entities.Add(other.rigidbody);
            else 
                _entities.Remove(other.rigidbody);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.rigidbody != null)
                _entities.Remove(other.rigidbody);
        }
    }
}