using Nevermind.Player.Controller;
using UnityEngine;

namespace Neverming.Player
{
    public class PlayerStatsManager : MonoBehaviour
    {
        public static PlayerStatsManager Instance {  get; private set; }

        private void Awake()
        {
            #region Singleton
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
            #endregion

        }

        private int countSlime;
        private PlayerControllersManager player;
        private void Start()
        {
            player = PlayerControllersManager.Instace;
        }

        public void EnterSlime()
        {
            countSlime++;

            player.ChangeControllerType(ControllerType.Omnidirectional);
        }

        public void ExitSlime(float pushOnExit) 
        {
            if (countSlime > 0)
            {
                countSlime--;

                if(countSlime <= 0)
                {
                    Vector2 inputDir =  player.OmnidirectionalController.GetInputDir();
                    player.ChangeControllerType(ControllerType.Default);
                    player.PlatformerController.Bounce(inputDir, pushOnExit);
                    countSlime = 0;
                }

            }
            else
            {
                Debug.LogError("You've never entered a slime, you shouldn't be able to exit one");
            }
        }

    }
}
