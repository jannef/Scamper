using UnityEngine;
using System.Collections.Generic;

namespace fi.tamk.game.theone.phys
{
    public class SceneManager : Singleton<SceneManager>
    {
        /**
         *  Flag for pausing the game.
         */
        public bool Pause = false;

        public Dictionary<GameObject, PGameBlock> GameObjectMap;
        public float _timescale = 1f;

        private float _deltaTime = 0f;
        /**
         *  Contains Time.deltaTime or 0 depending if the game is paused.
         */
        public float DeltaTime
        {
            get
            {
                if (Pause) return 0f;
                return _deltaTime;
            }
        }

        /**
         *  Is private to keep this solely a singleton class.
         */
        private SceneManager()
        {
            GameObjectMap = new Dictionary<GameObject, PGameBlock>();
        }

        // TODO: Here for testing purposes only!! REMOVE/MOVE AT SOME POINT!!
        void Update()
        {
            _deltaTime = Time.deltaTime * _timescale;

            if (Input.GetButtonDown("Jump"))
            {
                Pause = !Pause;
            }
        }

        public void PlayerDeathReset()
        {
            foreach (var t in GameObjectMap)
            {
                t.Value.ResetBlock();
            }
        }
    }
}
