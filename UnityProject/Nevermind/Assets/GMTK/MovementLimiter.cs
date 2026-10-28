using UnityEngine;

public class MovementLimiter : MonoBehaviour
{
    public bool CanJump {  get; set; }
    public bool CanMove { get; set; }

    private void Awake()
    {
        CanJump = true;
        CanMove = true;
    }
}
