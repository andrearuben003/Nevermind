using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer gateRenderer;
    
    [SerializeField]
    private GameObject openVisual;
    
    [SerializeField]
    private GameObject closeVisual;

    [SerializeField]
    private Collider2D gateCollider;

    private void Awake()
    {
        if (gateRenderer == null)
            gateRenderer = GetComponent<SpriteRenderer>();

        if (gateCollider == null)
            gateCollider = GetComponent<Collider2D>();

        CloseGate();
    }

    public void OpenGate()
    {
        openVisual.SetActive(true);
        closeVisual.SetActive(false);
        gateCollider.enabled = false;
        Debug.Log("Gate OPENED");
    }

    public void CloseGate()
    {
        openVisual.SetActive(false);
        closeVisual.SetActive(true);
        gateCollider.enabled = true;
        Debug.Log("Gate CLOSED");
    }
}
