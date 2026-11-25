using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Nevermind.Interactables
{
    [RequireComponent(typeof(Collider2D)), SelectionBase]
    public class PressurePlate : MonoBehaviour
    {
        public UnityEvent OnPressed;
        public UnityEvent OnReleased;
        public event Action<int, float> BodiesChanged;

        [SerializeField, Min(1)]
        private int requiredWeight;
        [SerializeField]
        private bool useRigidbodyMass = true;
        
        private List<Rigidbody2D> bodiesOnPlate = new();
        private bool isPressed;

        private void Start()
        {
            BodiesChanged?.Invoke(requiredWeight, 0);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            Rigidbody2D rb = other.attachedRigidbody;
            if (!rb) return;

            if (bodiesOnPlate.Contains(rb))
            {
                if (Mathf.Abs(rb.linearVelocityY) >= 0.001f)
                {
                    bodiesOnPlate.Remove(rb);
                    checkUnpress();
                }
            }
            else
            {
                if (Mathf.Abs(rb.linearVelocityY) < 0.001f)
                {
                    bodiesOnPlate.Add(rb);
                    checkPress();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Rigidbody2D rb = other.attachedRigidbody;
            if (!rb || 
                bodiesOnPlate.Contains(rb) ||
                Mathf.Abs(rb.linearVelocityY) >= 0.01f) return;
            
            bodiesOnPlate.Add(rb);
            
            checkPress();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Rigidbody2D rb = other.attachedRigidbody;
            if (!rb || !bodiesOnPlate.Contains(rb)) return;
            
            bodiesOnPlate.Remove(rb);
            
            checkUnpress();
        }
        
        private void checkPress()
        {
            bool press = pressCondition(out float currentPressure);
            
            BodiesChanged?.Invoke(requiredWeight, currentPressure);
            
            if (!isPressed && press)
            {
                OnPressed.Invoke();
                isPressed = true;
            }
        }

        private void checkUnpress()
        {
            bool unpress = !pressCondition(out float currentPressure);
            
            BodiesChanged?.Invoke(requiredWeight, currentPressure);
            
            if (isPressed && unpress)
            {
                OnReleased.Invoke();
                isPressed = false;
            }
        }

        private bool pressCondition(out float currentPressure)
        {
            if (!useRigidbodyMass)
                currentPressure = bodiesOnPlate.Count;
            else
                currentPressure = bodiesOnPlate.Aggregate(0, (float tot, Rigidbody2D body) => tot + body.mass);
            
            return currentPressure >= requiredWeight;
        }
    }
}
