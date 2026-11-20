using System.Collections;
using UnityEngine;

namespace Nevermind.Interactables
{
    public class LeverGraphics : InteractableGraphics
    {
        [SerializeField, Range(0, 360)]
        private float handleOffAngleDeg = 315f;
        [SerializeField, Range(0, 360)]
        private float handleOnAngleDeg = 45f;
        [SerializeField, Min(0)]
        private float animationSeconds = 0.75f;
        
        [SerializeField]
        private Transform handlePivot;
        
        private Lever lever;

        private int onRotationDir;
        private int offRotationDir;
        private float step;
        
        private void Awake()
        {
            lever = Interactable as Lever;
            onRotationDir = handleOnAngleDeg > handleOffAngleDeg ? -1 : 1;
            offRotationDir = handleOnAngleDeg > handleOffAngleDeg ? 1 : -1;
            step = Mathf.Abs(handleOnAngleDeg - handleOffAngleDeg) / animationSeconds;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            lever.OnActivated.AddListener(switchToOn);
            lever.OnDeactivated.AddListener(switchToOff);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            lever.OnActivated.RemoveListener(switchToOn);
            lever.OnDeactivated.RemoveListener(switchToOff);
        }

        private void switchToOn()
        {
            StopAllCoroutines();
            StartCoroutine(animateLever(handleOnAngleDeg, onRotationDir));
        }
        
        private void switchToOff()
        {
            StopAllCoroutines();
            StartCoroutine(animateLever(handleOffAngleDeg, offRotationDir));
        }

        private IEnumerator animateLever(float finalAngle, int dir)
        {
            Vector3 axis = handlePivot.forward * dir;
            while (Mathf.Abs(finalAngle - handlePivot.eulerAngles.z) > 1f)
            {
                handlePivot.Rotate(axis, step * Time.deltaTime);
                yield return null;
            }
            handlePivot.eulerAngles.Set(0, 0, finalAngle);
        }
    }
}
