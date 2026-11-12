
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RotaryController : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField]
    private Transform knobVisual;

    [Header("Rotation Settings")]
    [SerializeField]
    private float rotationSpeed = 90f;

    [SerializeField]
    private float minAngle = -90f;

    [SerializeField]
    private float maxAngle = 90f;

    [SerializeField]
    private float startAngle = 0f;

    [Header("Controlled Object")]
    [SerializeField]
    private Transform targetObject;

    private bool playerClose;

    private float currentAngle;

    private readonly HashSet<Collider2D> playerColliders = new();

    public float CurrentAngle => currentAngle;

    private void Awake()
    {
        currentAngle = Mathf.Clamp(startAngle, minAngle, maxAngle);
        UpdateRotation();
    }

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (!playerColliders.Add(other))
            return;

        if (playerColliders.Count == 1)
        {
            playerClose = true;
            Debug.Log("Player near Rotary Knob");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (!playerColliders.Remove(other))
            return;

        if (playerColliders.Count == 0)
        {
            playerClose = false;
            Debug.Log("Player left Rotary Knob");
        }
    }

    private void Update()
    {
        if (!playerClose)
            return;

        float direction = 0f;

        // Q: counterclockwise
        if (Input.GetKey(KeyCode.Q))
            direction += 1f;

        // E: clockwise
        if (Input.GetKey(KeyCode.E))
            direction -= 1f;

        if (direction == 0f)
            return;

        float newAngle = currentAngle +
            direction * rotationSpeed * Time.deltaTime;

        newAngle = Mathf.Clamp(newAngle, minAngle, maxAngle);

        if (Mathf.Approximately(newAngle, currentAngle))
            return;

        currentAngle = newAngle;

        UpdateRotation();

    }

    private void UpdateRotation()
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, currentAngle);

        if (knobVisual != null)
            knobVisual.localRotation = rotation;

        if (targetObject != null)
            targetObject.localRotation = rotation;
    }
}
