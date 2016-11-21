using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fi.tamk.game.theone.utils;
using fi.tamk.game.theone.menu;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Standard void no param event used to send messages.
    /// </summary>
    public delegate void SceneEvent();

    /// <summary>
    /// One scene lifetime singleton that handles game logic, blocks and time.
    /// </summary>
    /// /// <auth>Janne Forsell</auth>
    public class SceneManager : Singleton<SceneManager>
    {
        public LevelLoadController PersistentData;

        /// <summary>
        /// Pause property.
        /// </summary>
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

        /// <summary>
        /// Backing field for Pause.
        /// </summary>
        private bool _pause = false;

        public float TimeSinceLevelLoaded { get; private set; }

        /// <summary>
        /// Reference to the player rat game object.
        /// </summary>
        public GameObject PlayerGameObject;

        /// <summary>
        /// Event to blocks need to subscibe to to get level reset triggers.
        /// </summary>
        public event SceneEvent LevelResetEvent;

        /// <summary>
        /// Detecting when boxes are clicked.
        /// </summary>
        public event SceneEvent ClickEvent;

        /// <summary>
        /// Triggers ClickEvent.
        /// </summary>
        public void BoxClicked()
        {
            if (ClickEvent != null) ClickEvent();
        }

        /*
            Used to provide sin value based on time for character effects. This
            functionality was moved to shaders and then dropped.
        */
        //public float TimerPhase { get; private set; }

        /// <summary>
        /// Handles timescale of the game. Pausing relies on it.
        /// </summary>
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

        /// <summary>
        /// When paused, previous timescale is saved here.
        /// </summary>
        private float _timeScaleSaved = 1f;

        /// <summary>
        /// Property that gives deltaTime or 0, depending if the game is paused.
        /// </summary>
        public float DeltaTime
        {
            get
            {
                if (Pause) return 0f;
                return _deltaTime;
            }
        }

        /// <summary>
        /// Backing field for DeltaTime.
        /// </summary>
        private float _deltaTime = 0f;

        /// <summary>
        /// Map to get (c#)object when we know gameObject, to avoid runtime reflections
        /// </summary>
        public Dictionary<GameObject, PGameBlock> GameObjectMap;

        /// <summary>
        /// Sets up GameObjectMap and finds reference to player.
        /// </summary>
        private void Awake()
        {
            //TimerPhase = 0f;
            GameObjectMap = new Dictionary<GameObject, PGameBlock>();
            PlayerGameObject = FindObjectOfType<PPlayerBlock>().gameObject;
            PersistentData = FindObjectOfType<LevelLoadController>();

            if (PersistentData == null) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        /// <summary>
        /// Updates DeltaTime for the frame.
        /// </summary>
        private void Update()
        {
            _deltaTime = Time.deltaTime;
            TimeSinceLevelLoaded += DeltaTime;
            //TimerPhase = (Mathf.Sin(Time.timeSinceLevelLoad * 2f) + 1)/6;
        }

        /// <summary>
        /// Fires the scene resetting signal.
        /// </summary>
        public void PlayerDeathReset()
        {
            if (LevelResetEvent != null) LevelResetEvent();
        }
    }
}
