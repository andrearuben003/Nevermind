using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer buttonRenderer;

    private bool playerClose;

    private bool buttonPressed;

    [SerializeField]
    private UnityEvent onPressed = new();

    [SerializeField]
    private UnityEvent onReleased = new();

    private readonly HashSet<Collider2D> playerColliders = new();

    private void Awake()
    {
        if (buttonRenderer == null)
            buttonRenderer = GetComponent<SpriteRenderer>();

        buttonRenderer.color = Color.black;

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
            buttonRenderer.color = Color.yellow;
            Debug.Log("player near the button");
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
            if(!buttonPressed)
            {
                buttonRenderer.color = Color.black;
            }
            Debug.Log("player left the button");
            playerClose=false;
        }
    }

    //per gestire l'input da tastiera
    private void Update()
    {
            //uso GetKeyDown perche voglio avere l'input solo quando viene premuto
        if(playerClose && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F pressed");
            if(!buttonPressed)
            {
                Debug.Log("Button activated");
                onPressed.Invoke();
                buttonPressed=true;
                buttonRenderer.color = Color.green;
                StartCoroutine(ButtonTimer());
            }
        }
    }


    // every 5 seconds  
    private IEnumerator ButtonTimer()
    {
        print("5");
        for (int i = 4; i >= 0 ; i--)
        {
            yield return new WaitForSeconds(1);
            print(i);
        }

        print("timer finished");
        onReleased.Invoke();
        buttonPressed=false;
        if (playerClose)
            buttonRenderer.color = Color.yellow;    
        else
        {
            if(!buttonPressed)
                buttonRenderer.color = Color.black;
        }
    }

}
