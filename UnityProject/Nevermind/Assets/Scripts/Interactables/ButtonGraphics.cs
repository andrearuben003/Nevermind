using UnityEngine;

namespace Nevermind.Interactables
{
    public class ButtonGraphics : InteractableGraphics
    {
        [SerializeField]
        private Transform buttonPivot;
        
        private Button button;

        private void Awake()
        {
            button = Interactable as Button;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            button.TimerProgressed += onTimerProgress;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            button.TimerProgressed -= onTimerProgress;
        }

        private void onTimerProgress(float remaining)
        {
            buttonPivot.localScale = new Vector3(1, 1 - remaining, 1);
        }
    }
}
