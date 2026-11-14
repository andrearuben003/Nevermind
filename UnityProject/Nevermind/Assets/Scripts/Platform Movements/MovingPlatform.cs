using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nevermind.PlatformMovements
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] 
        private List<Vector3> pathTargets;
        
        [SerializeField, Min(0)] 
        private float speed = 1f;

        [SerializeField, Range(0.5f, 5f), Tooltip("How long the platform waits when it reaches a target.")]
        private float stopTimeSecs;
        
        [SerializeField, Tooltip("If true, will move backwards along the track after reaching the end instead of going to the starting position.")]
        private bool backtrack;

        [Tooltip("Tolerance value to consider the entity on top of the platform")]
        [SerializeField] private float topTolerance = 0.1f;

        private bool stopping;
        private int dir = 1;
        
        private Collider2D col;
        private int currentTargetIndex;

        //Entities on the platform
        private HashSet<Rigidbody2D> riderEntities;

        private void Awake()
        {
            col = GetComponent<Collider2D>();
            //_rb = GetComponent<Rigidbody2D>();
            riderEntities = new HashSet<Rigidbody2D>();
        }

        private void Start()
        {
            stopAndWait();
        }

        private void FixedUpdate()
        {
            if (stopping) return;
            
            Vector2 current = transform.position;
            Vector2 target = pathTargets[currentTargetIndex];
            float step = speed * Time.fixedDeltaTime;
            Vector2 next = Vector2.MoveTowards(current, target, step);
            Vector2 delta = next - current;

            transform.Translate(delta);
            //_rb.MovePosition(next);

            if (delta != Vector2.zero)
            {
                //Moves entities on the platform
                if(delta.y > 0f)
                    delta.y = 0f;
                
                foreach (Rigidbody2D e in riderEntities)
                    e.position += delta;
            }
            
            float reachTolerance = step / 2f;
            if (Vector2.Distance(next, target) <= reachTolerance)
            {
                // Target has been reached
                transform.position = pathTargets[currentTargetIndex];
                selectNextTarget();
                stopAndWait();
            }
        }

        private void selectNextTarget()
        {
            if (!backtrack)
            {
                currentTargetIndex++;
                currentTargetIndex %= pathTargets.Count;
                return;
            }

            if ((currentTargetIndex == pathTargets.Count - 1 && dir == 1) || (currentTargetIndex == 0 && dir == -1))
                dir *= -1;
            currentTargetIndex += dir;
        }

        private void stopAndWait()
        {
            stopping = true;

            Invoke(nameof(resume), stopTimeSecs);
        }
        
        private void resume() { stopping = false; }
        
        private void OnCollisionStay2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player") || other.rigidbody == null) 
                return;

            // Check if entity is on top of the platform
            if (other.collider.bounds.min.y >= col.bounds.max.y - topTolerance) 
                riderEntities.Add(other.collider.attachedRigidbody);
            else 
                riderEntities.Remove(other.collider.attachedRigidbody);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.rigidbody != null)
                riderEntities.Remove(other.collider.attachedRigidbody);
        }
    }
}