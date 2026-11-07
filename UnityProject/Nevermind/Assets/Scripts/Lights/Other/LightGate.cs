using Nevermind.Lights;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Collider2D),  typeof(SpriteRenderer))]
public class LightGate : LightTriggerTarget
{
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
        _collider.enabled = false;
        _spriteRenderer.color = _disabledColor;
    }

    public override void OnLightTriggerJustStoppedBeingHit()
    {
        _collider.enabled = true;
        _spriteRenderer.color = _enabledColor;
    }
}
