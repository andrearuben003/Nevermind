using System.Collections;
using UnityEngine;

namespace Nevermind.Interactables
{
    public class LeverGraphics : MonoBehaviour
    {
        private static readonly int OutlineEnabled = Shader.PropertyToID("_OutlineEnabled");

        [SerializeField, Range(0, 360)]
        private float handleOffAngleDeg = 315f;
        [SerializeField, Range(0, 360)]
        private float handleOnAngleDeg = 45f;
        [SerializeField, Min(0)]
        private float animationSeconds = 0.75f;
        
        [SerializeField]
        private Lever lever;
        [SerializeField]
        private SpriteRenderer baseRenderer;
        [SerializeField]
        private SpriteRenderer handleRenderer;
        [SerializeField]
        private Transform handlePivot;

        private int onRotationDir;
        private int offRotationDir;
        private float step;
        
        private void Awake()
        {
            onRotationDir = handleOnAngleDeg > handleOffAngleDeg ? -1 : 1;
            offRotationDir = handleOnAngleDeg > handleOffAngleDeg ? 1 : -1;
            step = Mathf.Abs(handleOnAngleDeg - handleOffAngleDeg) / animationSeconds;
        }

        private void OnEnable()
        {
            lever.playerEnteredInteractionRange += showOutline;
            lever.playerExitedInteractionRange += hideOutline;
            lever.OnActivated.AddListener(switchToOn);
            lever.OnDeactivated.AddListener(switchToOff);
        }

        private void OnDisable()
        {
            lever.playerEnteredInteractionRange -= showOutline;
            lever.playerExitedInteractionRange -= hideOutline;
            lever.OnActivated.RemoveListener(switchToOn);
            lever.OnDeactivated.RemoveListener(switchToOff);
        }

        private void showOutline() => setOutlineVisible(true);
        private void hideOutline() => setOutlineVisible(false);
        private void setOutlineVisible(bool visible)
        {
            baseRenderer.material.SetFloat(OutlineEnabled, visible ? 1 : 0);
            handleRenderer.material.SetFloat(OutlineEnabled, visible ? 1 : 0);
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
