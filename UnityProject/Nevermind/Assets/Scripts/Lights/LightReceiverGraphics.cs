using System;
using UnityEngine;

namespace Nevermind.Lights
{
    public class LightReceiverGraphics : MonoBehaviour
    {
        [SerializeField]
        private LightReceiver receiver;
        [SerializeField]
        private SpriteMask spriteMask;

        private void OnEnable()
        {
            receiver.OnActivate.AddListener(activate);
            receiver.OnDeactivate.AddListener(deactivate);
        }

        private void OnDisable()
        {
            receiver.OnActivate.RemoveListener(activate);
            receiver.OnDeactivate.RemoveListener(deactivate);
        }

        private void activate()
        {
            spriteMask.enabled = false;
        }
        private void deactivate()
        {
            spriteMask.enabled = true;
        }
    }
}
