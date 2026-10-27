using System.Collections.Generic;
using UnityEngine;

namespace Nevermind.Lights
{
    public class LightEmitterManager : MonoBehaviour
    {
        /// Emitters managed by this Manager
        [SerializeField] private List<LightEmitterDefault> defaultEmitters;
        [SerializeField] private List<LightEmitterToxic> toxicEmitters;
        [SerializeField] private List<LightEmitterSolid> solidEmitters;

        private void FixedUpdate()
        {
            // Update the beams in this specific order and updates the collider
            /* List of light emitters.
             * Note: the emitter render has a **very specific** order:
             * - solid, bad, good
             * - if the type is the same, the first in the list is rendered first
             */

            foreach (var emitter in solidEmitters)
            {
                emitter.BeamCast();
                emitter.UpdateBeamCollider();
            }   
            
            foreach (var emitter in toxicEmitters)
            {
                emitter.BeamCast();
                emitter.UpdateBeamCollider();
            }   
            
            foreach (var emitter in defaultEmitters)
            {
                emitter.BeamCast();
                emitter.UpdateBeamCollider();
            }   

        }

        private void Update()
        {
            // order is not important here, since the points are computed by the FixedUpdate
            
            foreach (var emitter in defaultEmitters)
                emitter.UpdateBeamRenderer();
            foreach (var emitter in toxicEmitters)
                emitter.UpdateBeamRenderer();
            foreach (var emitter in solidEmitters)
                emitter.UpdateBeamRenderer();
        }
    }
}