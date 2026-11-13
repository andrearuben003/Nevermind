using UnityEngine;

public class MovementLimiter : MonoBehaviour
{
    public bool CanJump {  get; set; }
    public bool CanMove { get; set; }
    
    public bool OnIce { get; set; }

    private void Awake()
    {
        CanJump = true;
        CanMove = true;
        OnIce = false;
    }
}
