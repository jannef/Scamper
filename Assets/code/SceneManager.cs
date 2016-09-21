using UnityEngine;
using System.Collections.Generic;

namespace fi.tamk.game.theone.phys
{
    public class SceneManager : Singleton<SceneManager>
    {
        /**
         *  Flag for pausing the game.
         */

        public int numero { get; private set; }

        public bool Pause
        {
            set
            {
                _pause = value;
                if (value)
                {
                    _timeScaleSaved = TimeScale;
                    TimeScale = 0f;
                } else
                {
                    TimeScale = _timeScaleSaved;
                }
            }

            get
            {
                return _pause;
            }
        }
        private bool _pause = false;

        public Dictionary<GameObject, PGameBlock> GameObjectMap;
        public float TimeScale
        {
            get
            {
                return Time.timeScale;
            }

            set
            {
                if (value >= 0f && value <= 5.0f)
                {
                    Time.timeScale = value;
                }
                else
                {
                    throw (new System.Exception("Time scale must be [0.0f, 5.0f]"));
                }
            }
        }
        private float _timeScaleSaved = 1f;

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
            _deltaTime = Time.deltaTime;

            if (Input.GetButtonDown("Jump"))
            {
                Pause = !Pause;
            }

            // For the luls
            // TimeScale += Input.GetAxis("Horizontal") * _deltaTime;
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
