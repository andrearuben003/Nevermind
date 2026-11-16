using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Nevermind.Interactables
{
    public class Button : Interactable
    {
        public UnityEvent OnPressed;
        public UnityEvent OnReleased;
        [SerializeField]
        private float activeDurationSeconds = 5f;
        
        public event Action<float> TimerProgressed;

        protected override void onSuccessfulInteraction()
        {
            StopAllCoroutines();
            StartCoroutine(pressButton());
        }

        private IEnumerator pressButton()
        {
            OnPressed?.Invoke();

            float timeRemaining = activeDurationSeconds;
            
            while (timeRemaining > 0f)
            {
                TimerProgressed?.Invoke(timeRemaining / activeDurationSeconds);
                
                yield return null;

                timeRemaining -= Time.deltaTime;
                timeRemaining = Mathf.Max(0f, timeRemaining);
            }

            OnReleased.Invoke();
        }
    }
}
