using UnityEngine;

namespace fi.tamk.game.theone
{
    public class SceneManager : Singleton<SceneManager>
    {
        /**
         *  Flag for pausing the game.
         *
         *  Game should probably not be paused in the middle of an update cycle... some objects might
         *  get to move 0-2 times extra depending on when the flow is interrupted/resumed compared to
         *  other objects.
         */
        public bool Pause = false;

        /**
         *  Contains Time.deltaTime or 0 depending if the game is paused.
         */
        public float DeltaTime
        {
            get
            {
                if (Pause) return 0f;
                return Time.deltaTime;
            }
        }

        /**
         *  Is private to keep this solely a singleton class.
         */
        private SceneManager()
        {

        }

        // TODO: Here for testing purposes only!! REMOVE/MOVE AT SOME POINT!!
        void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                Pause = !Pause;
            }
        }
    }
}
