using Nevermind.LevelSystem;
using Nevermind.Lights;
using UnityEngine;

namespace Nevermind.Player
{
    public class PlayerLightHandler : MonoBehaviour, IToxicLightTrigger
    {
        public void OnToxicLightBeamHit()
        {
            RespawnManager.Instance.Reload();
        }
    }
}
