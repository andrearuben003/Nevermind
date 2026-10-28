using System.Collections.Generic;
using UnityEngine;

namespace Nevermind.Lights
{
    public class LightEmitterManager : MonoBehaviour
    {
        /* List of light emitters.
         * Note: the emitter render has a very specific order:
         * - solid, bad, good
         * - if the type is the same, the first in the list is rendered first
         */

        /// Emitters managed by this Manager
        [SerializeField] private List<LightEmitter> emitters;

        private void FixedUpdate()
        {
            // Update the beams in this specific order
            // Note: it should be better to use specific lists instead of 3 loops for complexity,
            //    but there should not be too many emitters in one lever for that to matter

            // Solid
            for (int i = 0; i < emitters.Count; i++)
                if (emitters[i].LightType == LightType.Solid)
                    emitters[i].EmitterFixedUpdate();
            // Bad
            for (int i = 0; i < emitters.Count; i++)
                if (emitters[i].LightType == LightType.Toxic)
                    emitters[i].EmitterFixedUpdate();
            // Good
            for (int i = 0; i < emitters.Count; i++)
                if (emitters[i].LightType == LightType.Default)
                    emitters[i].EmitterFixedUpdate();
        }

        private void Update()
        {
            // order is not important here, since the points are computed by the FixedUpdate
            foreach (var emitter in emitters)
                emitter.EmitterUpdate();
        }
    }
}