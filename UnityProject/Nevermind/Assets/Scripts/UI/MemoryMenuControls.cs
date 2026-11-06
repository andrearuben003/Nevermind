using UnityEngine;

public class MemoryMenuControls : MonoBehaviour
{
    [SerializeField]
    private GameObject memoryMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            memoryMenu.SetActive(!memoryMenu.activeSelf);
    }
}
