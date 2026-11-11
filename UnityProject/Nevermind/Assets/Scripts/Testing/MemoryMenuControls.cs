using Nevermind.UI;
using UnityEngine;

namespace Nevermind.Testing
{
    [RequireComponent(typeof(MemoryMenu))]
    public class MemoryMenuControls : MonoBehaviour
    {
        [SerializeField]
        private bool verticalInput;
        
        private MemoryMenu menu;

        private void Awake()
        {
            menu = GetComponent<MemoryMenu>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
                menu.Toggle();

            if (verticalInput)
            {
                if (Input.GetKeyDown(KeyCode.K))
                    menu.SelectNext();
                if (Input.GetKeyDown(KeyCode.I))
                    menu.SelectPrevious();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.L))
                    menu.SelectNext();
                if (Input.GetKeyDown(KeyCode.J))
                    menu.SelectPrevious();
            }
            
            if (Input.GetKeyDown(KeyCode.O))
                menu.ForgetSelected();
        }
    }
}
