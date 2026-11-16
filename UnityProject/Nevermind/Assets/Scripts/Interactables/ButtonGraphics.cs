using UnityEngine;

namespace Nevermind.Interactables
{
    public class ButtonGraphics : MonoBehaviour
    {
        private static readonly int OutlineEnabled = Shader.PropertyToID("_OutlineEnabled");
        
        [SerializeField]
        private Button button;
        [SerializeField]
        private SpriteRenderer baseRenderer;
        [SerializeField]
        private SpriteRenderer buttonRenderer;
        [SerializeField]
        private Transform buttonPivot;

        private void OnEnable()
        {
            button.playerEnteredInteractionRange += showOutline;
            button.playerExitedInteractionRange += hideOutline;
            button.TimerProgressed += onTimerProgress;
        }

        private void OnDisable()
        {
            button.playerEnteredInteractionRange -= showOutline;
            button.playerExitedInteractionRange -= hideOutline;
            button.TimerProgressed -= onTimerProgress;
        }

        private void showOutline() => setOutlineVisible(true);
        private void hideOutline() => setOutlineVisible(false);
        private void setOutlineVisible(bool visible)
        {
            baseRenderer.material.SetFloat(OutlineEnabled, visible ? 1 : 0);
            buttonRenderer.material.SetFloat(OutlineEnabled, visible ? 1 : 0);
        }

        private void onTimerProgress(float remaining)
        {
            buttonPivot.localScale = new Vector3(1, 1 - remaining, 1);
        }
    }
}
