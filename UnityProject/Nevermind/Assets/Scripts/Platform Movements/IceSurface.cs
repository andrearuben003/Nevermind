using System;
using System.Collections.Generic;
using Nevermind.MemorySystem;
using TarodevController;
using UnityEngine;

public class IceSurface : MonoBehaviour
{

    [SerializeField] private PhysicsMaterial2D iceMaterial;
    
    private Dictionary<GameObject, PhysicsMaterial2D> _bodies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _bodies = new Dictionary<GameObject, PhysicsMaterial2D>();
    }

    void OnEnable()
    {//Check for any colliders already on the surface
        List<Collider2D> cols = new();
        GetComponent<Collider2D>().Overlap(cols);

        foreach (Collider2D col in cols) {
            IceEffectEnable(col);
        }
    }

    void OnDisable()
    {//Check for are any colliders still on the surface
        List<Collider2D> cols = new();
        GetComponent<Collider2D>().Overlap(cols);

        foreach (Collider2D col in cols) {
            IceEffectDisable(col);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!enabled)
            return;
        
        IceEffectEnable(other);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!enabled)
            return;

        IceEffectDisable(other);
    }

    private void IceEffectEnable(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerController>().SetIce(true);
        }
        else if (other.gameObject != null && other.attachedRigidbody.bodyType == RigidbodyType2D.Dynamic) {
            _bodies[other.gameObject] = other.gameObject.GetComponent<Collider2D>().sharedMaterial;
            other.gameObject.GetComponent<Collider2D>().sharedMaterial = iceMaterial;
        }
    }

    private void IceEffectDisable(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerController>().SetIce(false);
        }
        else if (other.gameObject != null && other.attachedRigidbody.bodyType == RigidbodyType2D.Dynamic){
            if (_bodies.TryGetValue(other.gameObject, out var mat)) {
                other.gameObject.GetComponent<Collider2D>().sharedMaterial = mat;
                _bodies.Remove(other.gameObject);
            }
        }
    }
}
