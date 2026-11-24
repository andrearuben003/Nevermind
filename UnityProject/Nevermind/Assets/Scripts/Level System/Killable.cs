using System;
using UnityEngine;

namespace Nevermind.LevelSystem
{
    public class Killable : MonoBehaviour
    {
        public event Action Killed;

        /// Invoke the Killed event and destroy the gameObject.
        public virtual void Kill()
        {
            Killed?.Invoke();
            Destroy(gameObject);
        }
    }
}