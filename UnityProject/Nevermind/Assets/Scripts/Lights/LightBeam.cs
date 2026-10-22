using UnityEngine;

namespace Nevermind.Lights
{
    /// <summary>
    /// Most of the logic is defined in the LightEmitter class
    /// </summary>
    public class LightBeam : MonoBehaviour
    {
        /// <summary>
        /// This beam's emitter. Every beam is guaranteed to have one.
        /// </summary>
        [SerializeField] public LightEmitter emitter;

        private void Awake()
        {
            if (!emitter)
                emitter = GetComponentInParent<LightEmitter>();
            if (!emitter)
                Debug.LogError(emitter.name + " is not a LightBeam");
        }
    }
}