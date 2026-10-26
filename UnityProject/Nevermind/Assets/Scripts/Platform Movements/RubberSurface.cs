using System;
using System.Collections.Generic;
using Nevermind.MemorySystem;
using TarodevController;
using UnityEngine;

public class RubberSurface : MonoBehaviour
{
    [SerializeField] float bounceSpeed = 10;

    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = this.gameObject.GetComponentInParent<Transform>().GetComponentInParent<Rigidbody2D>();
        Debug.Log(_rb.gameObject.name);
    }

    void OnEnable()
    {
        List<Collider2D> cols = new();
        GetComponent<Collider2D>().Overlap(cols);

        foreach (Collider2D col in cols) {
            GenerateBounce(col);
        }

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!enabled)
            return;
        
        GenerateBounce(other);
    }

    private void GenerateBounce(Collider2D other)
    {
        if (other.attachedRigidbody == _rb || other.attachedRigidbody == null)
            return;
        
        Vector2 velocity = this.transform.up * bounceSpeed;
        
        if(_rb.bodyType == RigidbodyType2D.Dynamic)
            _rb.linearVelocity = -velocity;
        
        if(other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<PlayerController>().Bounce(velocity);
        else if(other.attachedRigidbody.bodyType == RigidbodyType2D.Dynamic)
            other.attachedRigidbody.linearVelocity = velocity;
    }
}
