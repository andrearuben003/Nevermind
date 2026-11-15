using Nevermind.Lights;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

[SelectionBase]
[RequireComponent(typeof(Collider2D),  typeof(SpriteRenderer))]
public class LightGate : LightTriggerTarget
{
    [SerializeField] private int triggerCountToOpen = 1;
    private int currentTriggerCount = 0;
    
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private Color _enabledColor;
    private Color _disabledColor;

    public void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Color
        _enabledColor = Color.lawnGreen;
        Color c = Color.lawnGreen;
        c.a = 0.2f;
        _disabledColor = c;
        _spriteRenderer.color = _enabledColor;
    }
    
    public override void OnLightTriggerJustHit()
    {
        currentTriggerCount++;
        if (currentTriggerCount < triggerCountToOpen) return;
        _collider.enabled = false;
        _spriteRenderer.color = _disabledColor;
    }

    public override void OnLightTriggerJustStoppedBeingHit()
    {
        currentTriggerCount--;
        // Assert.Greater(currentTriggerCount, 0); // this should not happen, this actually happens because of Edoardo
        if (currentTriggerCount >= triggerCountToOpen) return;
        _collider.enabled = true;
        _spriteRenderer.color = _enabledColor;
    }
}
