using Nevermind.Material;
using System.Collections.Generic;
using UnityEngine;

namespace Nevermind.PlatformMovements
{
    public class PlatformTranslation2 : MonoBehaviour
    {
        [SerializeField] private List<Transform> pathTargets;
        [SerializeField] private float speed = 1f;

        [Tooltip("Tolerance value to consider the entity on top of the platform")]
        [SerializeField] private float topTolerance = 0.1f;

        private Collider2D _collider;
        private int _currentTargetIndex = 0;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();

            _currentTargetIndex++;
            _currentTargetIndex %= pathTargets.Count;
        }

        private void Update()
        {
            Vector2 current = transform.position;
            Vector2 target = pathTargets[_currentTargetIndex].position;

            transform.position = Vector2.MoveTowards(current, target, speed * Time.deltaTime);

            if(Vector2.Distance(current, target) <= 0.0001f){
                _currentTargetIndex++;
                _currentTargetIndex %= pathTargets.Count;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.collider.CompareTag("Box") && !other.collider.CompareTag("Player"))
            {
                other.transform.parent = transform;
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            other.transform.parent = transform;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            other.transform.parent = null;
        }

    }
}