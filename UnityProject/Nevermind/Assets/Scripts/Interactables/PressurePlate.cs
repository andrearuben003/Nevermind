using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PressurePlate : MonoBehaviour
{

    [SerializeField] 
    private SpriteRenderer plateRenderer;

    [SerializeField]
    private UnityEvent onPressed = new();

    [SerializeField]
    private UnityEvent onReleased = new();

    private readonly HashSet<Collider2D> playerColliders = new();

    //colore iniziale rosso
    private void Awake()
    {
        if (plateRenderer == null)
            plateRenderer = GetComponent<SpriteRenderer>();

        plateRenderer.color = Color.red;
    }
private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player"))
            return;

         if (!playerColliders.Add(collider))
            return;

        if (playerColliders.Count == 1)
        {
            plateRenderer.color = Color.green;
            Debug.Log("Pressure Plate ACTIVATED");
            onPressed.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player"))
            return;

        playerColliders.Remove(collider);

        if (playerColliders.Count == 0)
        {
            plateRenderer.color = Color.red;
            Debug.Log("Pressure Plate DEACTIVATED");
            onReleased.Invoke();
        }
    }
}
