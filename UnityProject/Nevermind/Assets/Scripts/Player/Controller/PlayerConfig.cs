using UnityEngine;

namespace Nevermind.Player.Controller
{
    [CreateAssetMenu]
    public class PlayerConfig : ScriptableObject
    {
        [Header("MOVEMENT")]
        [SerializeField, Range(0f, 20f)][Tooltip("Maximum movement speed")] public float maxSpeed = 10f;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to reach max speed")] public float maxAcceleration = 52f;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop after letting go")] public float maxDecceleration = 52f;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop when changing direction")] public float maxTurnSpeed = 80f;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to reach max speed when in mid-air")] public float maxAirAcceleration;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop in mid-air when no direction is used")] public float maxAirDeceleration;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop when changing direction when in mid-air")] public float maxAirTurnSpeed = 80f;
        [SerializeField][Tooltip("Friction to apply against movement on stick")] public float friction;
        [Tooltip("When false, the charcter will skip acceleration and deceleration and instantly move and stop")] public bool useAcceleration;


        [Header("JUMP")]
        [SerializeField, Range(2f, 5.5f)][Tooltip("Maximum jump height")] public float jumpHeight = 7.3f;
        [SerializeField, Range(0.2f, 1.25f)][Tooltip("How long it takes to reach that height before coming back down")] public float timeToJumpApex;
        [SerializeField, Range(0f, 5f)][Tooltip("Gravity multiplier to apply when going up")] public float upwardMovementMultiplier = 1f;
        [SerializeField, Range(1f, 10f)][Tooltip("Gravity multiplier to apply when coming down")] public float downwardMovementMultiplier = 6.17f;
        [SerializeField, Range(0, 1)][Tooltip("How many times can you jump in the air?")] public int maxAirJumps = 0;
        [Tooltip("Should the character drop when you let go of jump?")] public bool variablejumpHeight;
        [SerializeField, Range(1f, 10f)][Tooltip("Gravity multiplier when you let go of jump")] public float jumpCutOff;
        [SerializeField][Tooltip("The fastest speed the character can fall")] public float speedLimit;
        [SerializeField, Range(0f, 0.3f)][Tooltip("How long should coyote time last?")] public float coyoteTime = 0.15f;
        [SerializeField, Range(0f, 0.3f)][Tooltip("How far from ground should we cache your jump?")] public float jumpBuffer = 0.15f;


        [Header("GROUND")]
        [SerializeField][Tooltip("Length of the ground-checking collider")] public float groundLength = 0.95f;
        [SerializeField][Tooltip("Distance between the ground-checking colliders")] public Vector3 colliderOffset;
        [SerializeField][Tooltip("Which layers are read as the ground")] public LayerMask groundLayer;

    }
}
