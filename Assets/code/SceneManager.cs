using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fi.tamk.game.theone.utils;

namespace fi.tamk.game.theone.phys
{
    public class SceneManager : Singleton<SceneManager>
    {
        #region PauseAndTime
        /**
         *  Flag for pausing the game.
         */
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

        public GameObject PlayerGameObject;

        public float TimerPhase { get; private set; }

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
        #endregion

        /**
         * Map to get (c#)object when we know gameObject, to avoid runtime reflections
         */
        public Dictionary<GameObject, PGameBlock> GameObjectMap;

        private void Awake()
        {
            TimerPhase = 0f;
            GameObjectMap = new Dictionary<GameObject, PGameBlock>();
            PlayerGameObject = FindObjectOfType<PPlayerBlock>().gameObject;
        }

        // TODO: Here for testing purposes only!! REMOVE/MOVE AT SOME POINT!!
        private void Update()
        {
            _deltaTime = Time.deltaTime;
            TimerPhase = (Mathf.Sin(Time.timeSinceLevelLoad * 2f) + 1)/6;

            if (Input.GetButtonDown("Jump"))
            {
                Pause = !Pause;
            }

            // For the luls
            // TimeScale += Input.GetAxis("Horizontal") * _deltaTime;
        }

        /**
         *  Resets the scene on player death.
         */
        public void PlayerDeathReset()
        {
            foreach (var t in GameObjectMap)
            {
                t.Value.ResetBlock();                
            }
        }
    }
}
