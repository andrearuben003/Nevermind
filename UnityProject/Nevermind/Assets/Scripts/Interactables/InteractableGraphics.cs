using UnityEngine;

namespace Nevermind.Interactables
{
    public class InteractableGraphics : MonoBehaviour
    {
        private static readonly int OutlineEnabled = Shader.PropertyToID("_OutlineEnabled");

        [SerializeField]
        protected Interactable Interactable;

        [SerializeField,
         Tooltip("Sprite renderers with the outline material that will have their outlines enabled when the player can interact with the object.")]
        private SpriteRenderer[] objectSprites;

        protected virtual void OnEnable()
        {
            Interactable.Selected += showOutline;
            Interactable.Deselected += hideOutline;
        }

        protected virtual void OnDisable()
        {
            Interactable.Selected -= showOutline;
            Interactable.Deselected -= hideOutline;
        }
        
        private void showOutline() => setOutlineVisible(true);
        private void hideOutline() => setOutlineVisible(false);
        private void setOutlineVisible(bool visible)
        {
            foreach (SpriteRenderer sR in objectSprites)
                sR.material.SetFloat(OutlineEnabled, visible ? 1 : 0);
        }
    }
}
