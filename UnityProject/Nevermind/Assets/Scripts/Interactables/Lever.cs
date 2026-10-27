using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
   [SerializeField]
    private SpriteRenderer leverRenderer;

    private bool playerClose;

    private bool leverActive;

    [SerializeField]
    private UnityEvent onActivated = new();

    [SerializeField]
    private UnityEvent onDeactivated = new();

    private readonly HashSet<Collider2D> playerColliders = new();

    private void Awake()
    {
        if (leverRenderer == null)
            leverRenderer = GetComponent<SpriteRenderer>();

        leverRenderer.color = Color.black;

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player"))
            return;

        if (!playerColliders.Add(collider))
            return;

        if (playerColliders.Count == 1)
        {
            playerClose=true;
            if(!leverActive)
                leverRenderer.color = Color.yellow;
            Debug.Log("player near the lever");
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player"))
            return;

        if (!playerColliders.Remove(collider))
            return;

        if (playerColliders.Count == 0)
        {
            if(!leverActive)
            {
                leverRenderer.color = Color.black;
            }
            Debug.Log("player left the lever");
            playerClose=false;
        }
    }

    private void Update()
    {
            //uso GetKeyDown perche voglio avere l'input solo quando viene premuto
        if(playerClose && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F pressed");
            if(!leverActive)
            {
                Debug.Log("lever activated");
                onActivated.Invoke();
                leverActive=true;
                leverRenderer.color = Color.green;
            }
            else
            {
                Debug.Log("lever deactivated");
                onDeactivated.Invoke();
                leverActive=false;
                leverRenderer.color = Color.yellow;
            }
        }
    }
}
