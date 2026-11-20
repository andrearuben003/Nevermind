using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nevermind.Interactables
{
    public class PressurePlateGraphics : MonoBehaviour
    {
        private static readonly Color PressedColor = Color.greenYellow;
        private static readonly Color ReleasedColor = Color.lightGoldenRod;
        
        [SerializeField]
        private PressurePlate pressurePlate;
        [SerializeField]
        private Transform platePivot;
        [SerializeField]
        private TextMeshProUGUI pressurePlateText;
        [SerializeField]
        private Image pressurePlateImage;

        private void OnEnable()
        {
            pressurePlate.OnPressed.AddListener(onPress);
            pressurePlate.OnReleased.AddListener(onRelease);
            pressurePlate.BodiesChanged += updateText;
        }

        private void OnDisable()
        {
            pressurePlate.OnPressed.RemoveListener(onPress);
            pressurePlate.OnReleased.RemoveListener(onRelease);
            pressurePlate.BodiesChanged -= updateText;
        }

        private void onPress()
        {
            platePivot.localScale = new Vector3(1, 0.2f, 1);
            pressurePlateImage.color = PressedColor;
        }

        private void onRelease()
        {
            platePivot.localScale = new Vector3(1, 1, 1);
            pressurePlateImage.color = ReleasedColor;
        }
        
        private void updateText(int requiredWeight, float currentWeight)
        {
            pressurePlateText.text = Mathf.Max(0, requiredWeight - currentWeight).ToString("0");
        }
    }
}
